namespace CourseShop.Application.Ordering.Dtos;

/// <summary>Модель відповіді для замовлення (DTO прикладного рівня).</summary>
public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string Status,
    string Total,
    IReadOnlyList<OrderItemResponse> Items);

public sealed record OrderItemResponse(string ProductName, string Price, int Quantity);
