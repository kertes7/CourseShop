using CourseShop.Domain.Common;

namespace CourseShop.Domain.Ordering;

/// <summary>
/// Позиція замовлення. Є частиною агрегату Order і не існує окремо від нього.
/// </summary>
public sealed class OrderItem : Entity
{
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public OrderItem(string productName, Money unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Назва товару обов'язкова.", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentException("Кількість має бути більшою за нуль.", nameof(quantity));

        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Money LineTotal => UnitPrice.Multiply(Quantity);
}
