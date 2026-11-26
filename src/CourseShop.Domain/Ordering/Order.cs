using CourseShop.Domain.Common;
using CourseShop.Domain.Ordering.Events;

namespace CourseShop.Domain.Ordering;

/// <summary>
/// Агрегат "Замовлення" (Aggregate Root) у bounded context Order Management.
/// Містить лише бізнес-правила та інваріанти й не залежить від інфраструктури.
/// На користувача посилається лише через ідентифікатор (CustomerId),
/// не знаючи про внутрішню модель контексту User Management.
/// </summary>
public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.New;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // для ORM / серіалізації

    public Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Замовлення має належати користувачу.", nameof(customerId));
        CustomerId = customerId;
    }

    public void AddItem(string productName, Money unitPrice, int quantity)
    {
        EnsureEditable();
        _items.Add(new OrderItem(productName, unitPrice, quantity));
    }

    /// <summary>Підтвердження замовлення — інваріант: має бути хоча б одна позиція.</summary>
    public void Confirm()
    {
        EnsureEditable();
        if (_items.Count == 0)
            throw new InvalidOperationException("Не можна підтвердити порожнє замовлення.");
        Status = OrderStatus.Confirmed;
        // Реєструємо доменну подію замість прямого виклику сервісів-споживачів.
        RaiseDomainEvent(new OrderConfirmedEvent(Id, CustomerId, Total().Amount));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Замовлення вже скасовано.");
        Status = OrderStatus.Cancelled;
    }

    public Money Total()
        => _items.Aggregate(Money.Zero(), (sum, item) => sum.Add(item.LineTotal));

    private void EnsureEditable()
    {
        if (Status != OrderStatus.New)
            throw new InvalidOperationException("Змінювати можна лише нове замовлення.");
    }
}
