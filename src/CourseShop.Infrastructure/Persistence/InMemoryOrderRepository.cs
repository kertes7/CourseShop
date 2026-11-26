using CourseShop.Domain.Ordering;

namespace CourseShop.Infrastructure.Persistence;

/// <summary>
/// Репозиторій замовлень над InMemoryDatabase. AddAsync лише реєструє зміну
/// (stage); фактичне збереження виконує Unit of Work під час SaveChangesAsync.
/// Так розмежовано відповідальність репозиторію (колекція) та UoW (транзакція).
/// </summary>
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly InMemoryDatabase _db;

    public InMemoryOrderRepository(InMemoryDatabase db) => _db = db;

    public Task AddAsync(Order order, CancellationToken ct = default)
    {
        _db.StageOrder(order);          // зміна зареєстрована, але ще не збережена
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_db.Orders.TryGetValue(id, out var order) ? order : null);

    public Task<IReadOnlyList<Order>> GetByCustomerAsync(Guid customerId, CancellationToken ct = default)
    {
        IReadOnlyList<Order> result = _db.Orders.Values
            .Where(o => o.CustomerId == customerId)
            .ToList();
        return Task.FromResult(result);
    }
}
