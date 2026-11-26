using CourseShop.Application.Common;
using CourseShop.Application.Ordering;
using CourseShop.Application.Ordering.CreateOrder;
using CourseShop.Application.Ordering.Dtos;
using CourseShop.Domain.Common;
using CourseShop.Domain.Ordering;
using Microsoft.AspNetCore.Mvc;

namespace CourseShop.WebApi.Controllers;

/// <summary>
/// Контролер замовлень. Відповідає за прийом HTTP-запитів і делегування.
/// Для операції читання застосовано кешування (ПЗ9) з fallback до сховища
/// та інвалідацією після зміни даних.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrder;
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDemoDataProvider _demo;
    private readonly ICacheService _cache;

    private static string CacheKey(Guid id) => $"order:{id}";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    public OrdersController(CreateOrderHandler createOrder, IOrderRepository orders,
        IUnitOfWork unitOfWork, IDemoDataProvider demo, ICacheService cache)
    {
        _createOrder = createOrder;
        _orders = orders;
        _unitOfWork = unitOfWork;
        _demo = demo;
        _cache = cache;
    }

    [HttpGet("demo-customer")]
    public IActionResult DemoCustomer() => Ok(new { customerId = _demo.GetDemoCustomerId() });

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        try
        {
            var id = await _createOrder.HandleAsync(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { orderId = id });
        }
        catch (FluentValidation.ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new { title = "Помилка валідації", errors });
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        // 1) Спроба отримати з кешу.
        var cached = await _cache.GetAsync<OrderResponse>(CacheKey(id), ct);
        if (cached is not null)
            return Ok(new { source = "cache", order = cached });

        // 2) Fallback до сховища.
        var order = await _orders.GetByIdAsync(id, ct);
        if (order is null) return NotFound();

        var dto = OrderMapper.ToResponse(order);
        await _cache.SetAsync(CacheKey(id), dto, CacheTtl, ct);   // наповнення кешу
        return Ok(new { source = "database", order = dto });
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var order = await _orders.GetByIdAsync(id, ct);
        if (order is null) return NotFound();

        try { order.Cancel(); }
        catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }

        await _unitOfWork.SaveChangesAsync(ct);
        await _cache.RemoveAsync(CacheKey(id), ct);   // інвалідація кешу після зміни
        return Ok(new { status = order.Status.ToString() });
    }
}
