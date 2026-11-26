using System.Collections.Concurrent;
using CourseShop.Domain.Ordering;

namespace CourseShop.Infrastructure.Persistence;

/// <summary>
/// Спрощена "база даних" у пам'яті. Зберігає закомічені сутності та чергу змін,
/// що очікують збереження. Імітує поведінку контексту ORM, де зміни застосовуються
/// лише під час виклику SaveChanges (Unit of Work).
/// </summary>
public sealed class InMemoryDatabase
{
    public ConcurrentDictionary<Guid, Order> Orders { get; } = new();

    private readonly List<Order> _pendingOrders = new();

    public void StageOrder(Order order)
    {
        lock (_pendingOrders) _pendingOrders.Add(order);
    }

    /// <summary>Застосовує всі відкладені зміни. Повертає кількість збережених записів.</summary>
    public int Commit()
    {
        lock (_pendingOrders)
        {
            foreach (var order in _pendingOrders)
                Orders[order.Id] = order;
            var count = _pendingOrders.Count;
            _pendingOrders.Clear();
            return count;
        }
    }
}
