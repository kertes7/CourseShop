namespace CourseShop.Application.Customers.Dtos;

/// <summary>DTO користувача для зовнішніх шарів (без доменної поведінки).</summary>
public sealed record CustomerResponse(Guid Id, string FullName, string Email, string? Address);
