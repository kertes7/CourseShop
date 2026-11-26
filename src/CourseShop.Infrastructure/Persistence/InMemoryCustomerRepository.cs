using System.Collections.Concurrent;
using CourseShop.Application.Common;
using CourseShop.Domain.Customers;

namespace CourseShop.Infrastructure.Persistence;

/// <summary>Реалізація репозиторію користувачів у пам'яті. Також надає демо-дані (IDemoDataProvider).</summary>
public sealed class InMemoryCustomerRepository : ICustomerRepository, IDemoDataProvider
{
    private readonly ConcurrentDictionary<Guid, Customer> _store = new();

    public InMemoryCustomerRepository()
    {
        // демонстраційний користувач, щоб API можна було одразу перевірити
        var demo = new Customer("Іван Гнетило", "customer@example.com");
        _store[demo.Id] = demo;
        SeededCustomerId = demo.Id;
    }

    /// <summary>Id попередньо створеного користувача (для демонстрації).</summary>
    public Guid SeededCustomerId { get; }

    /// <summary>Реалізація IDemoDataProvider (DIP): вебшар звертається через абстракцію.</summary>
    public Guid GetDemoCustomerId() => SeededCustomerId;

    public Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        _store[customer.Id] = customer;
        return Task.CompletedTask;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_store.TryGetValue(id, out var c) ? c : null);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_store.ContainsKey(id));
}
