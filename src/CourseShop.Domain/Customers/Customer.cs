using CourseShop.Domain.Common;

namespace CourseShop.Domain.Customers;

/// <summary>
/// Агрегат "Користувач" у bounded context User Management.
/// Не містить логіки обробки замовлень — відповідає лише за дані користувача.
/// </summary>
public sealed class Customer : Entity
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public Address? Address { get; private set; }

    private Customer() { FullName = string.Empty; Email = string.Empty; }

    public Customer(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Ім'я користувача обов'язкове.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Некоректна електронна пошта.", nameof(email));

        FullName = fullName;
        Email = email;
    }

    public void SetAddress(Address address) => Address = address;
}
