namespace CourseShop.Application.Ordering.CreateOrder;

/// <summary>Вхідні дані сценарію створення замовлення (DTO прикладного рівня).</summary>
public sealed record CreateOrderCommand(Guid CustomerId, IReadOnlyList<CreateOrderItem> Items);

public sealed record CreateOrderItem(string ProductName, decimal UnitPrice, int Quantity);
