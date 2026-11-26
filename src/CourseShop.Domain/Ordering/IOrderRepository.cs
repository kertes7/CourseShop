namespace CourseShop.Domain.Ordering;

/// <summary>
/// Контракт репозиторію замовлень. Оголошений у доменному шарі як абстракція;
/// конкретна реалізація знаходиться в шарі Infrastructure (інверсія залежностей).
/// </summary>
public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Order>> GetByCustomerAsync(Guid customerId, CancellationToken ct = default);
}
