using CourseShop.Domain.Common;

namespace CourseShop.Domain.Customers;

/// <summary>
/// Об'єкт-значення "адреса" у bounded context User Management.
/// Поняття адреси тут пов'язане саме з користувачем, а не з платіжною логікою.
/// </summary>
public sealed class Address : ValueObject
{
    public string City { get; }
    public string Street { get; }
    public string PostalCode { get; }

    public Address(string city, string street, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Місто обов'язкове.", nameof(city));
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Вулиця обов'язкова.", nameof(street));

        City = city;
        Street = street;
        PostalCode = postalCode ?? string.Empty;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return PostalCode;
    }

    public override string ToString() => $"{PostalCode}, {City}, {Street}".Trim(',', ' ');
}
