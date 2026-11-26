namespace CourseShop.Domain.Customers;

/// <summary>Контракт репозиторію користувачів (абстракція доменного шару).</summary>
public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken ct = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}
