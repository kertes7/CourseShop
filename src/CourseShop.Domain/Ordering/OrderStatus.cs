namespace CourseShop.Domain.Ordering;

/// <summary>Статус замовлення у контексті Order Management.</summary>
public enum OrderStatus
{
    New = 0,
    Confirmed = 1,
    Cancelled = 2
}
