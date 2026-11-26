using CourseShop.Application.Customers.Dtos;
using CourseShop.Domain.Customers;

namespace CourseShop.Application.Customers;

/// <summary>
/// Відповідає лише за мапінг доменної сутності Customer у DTO.
/// Доменна модель не передається у зовнішні шари напряму.
/// </summary>
public static class CustomerMapper
{
    public static CustomerResponse ToResponse(Customer customer) => new(
        customer.Id,
        customer.FullName,
        customer.Email,
        customer.Address?.ToString());
}
