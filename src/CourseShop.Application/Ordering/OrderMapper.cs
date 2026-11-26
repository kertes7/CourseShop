using CourseShop.Application.Ordering.Dtos;
using CourseShop.Domain.Ordering;

namespace CourseShop.Application.Ordering;

/// <summary>
/// Відповідає лише за перетворення доменної моделі Order у DTO (SRP).
/// Раніше ця логіка знаходилась у контролері разом з обробкою HTTP —
/// що порушувало принцип єдиної відповідальності.
/// </summary>
public static class OrderMapper
{
    public static OrderResponse ToResponse(Order order) => new(
        order.Id,
        order.CustomerId,
        order.Status.ToString(),
        order.Total().ToString(),
        order.Items
            .Select(i => new OrderItemResponse(i.ProductName, i.UnitPrice.ToString(), i.Quantity))
            .ToList());
}
