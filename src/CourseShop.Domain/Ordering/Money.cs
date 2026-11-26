using CourseShop.Domain.Common;

namespace CourseShop.Domain.Ordering;

/// <summary>
/// Об'єкт-значення "грошова сума". Інкапсулює суму та валюту,
/// забороняє від'ємні значення та операції з різними валютами.
/// </summary>
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "UAH")
    {
        if (amount < 0)
            throw new ArgumentException("Грошова сума не може бути від'ємною.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Валюта обов'язкова.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency = "UAH") => new(0, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int quantity) => new(Amount * quantity, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Не можна виконувати операції з різними валютами.");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
