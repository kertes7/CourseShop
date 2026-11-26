using CourseShop.Domain.Common;

namespace CourseShop.Domain.Ordering.Events;

/// <summary>
/// Подія "Замовлення підтверджено". Відображає значущу зміну стану домену
/// й містить мінімальний обсяг даних (ідентифікатори та підсумок).
/// </summary>
public sealed class OrderConfirmedEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public decimal TotalAmount { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public OrderConfirmedEvent(Guid orderId, Guid customerId, decimal totalAmount)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }
}
