using CourseShop.Application.Common;
using CourseShop.Application.Customers;
using CourseShop.Domain.Customers;
using Microsoft.AspNetCore.Mvc;

namespace CourseShop.WebApi.Controllers;

/// <summary>
/// Контролер користувачів. Повертає назовні виключно DTO (CustomerResponse);
/// доменна сутність Customer не передається у зовнішній шар.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customers;
    private readonly IDemoDataProvider _demo;

    public CustomersController(ICustomerRepository customers, IDemoDataProvider demo)
    {
        _customers = customers;
        _demo = demo;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(id, ct);
        return customer is null ? NotFound() : Ok(CustomerMapper.ToResponse(customer));
    }

    [HttpGet("demo")]
    public async Task<IActionResult> Demo(CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(_demo.GetDemoCustomerId(), ct);
        return customer is null ? NotFound() : Ok(CustomerMapper.ToResponse(customer));
    }
}
