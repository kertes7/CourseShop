using CourseShop.Application.Common.Events;
using CourseShop.Domain.Common;
using CourseShop.Domain.Customers;
using CourseShop.Domain.Ordering;
using FluentValidation;

namespace CourseShop.Application.Ordering.CreateOrder;

/// <summary>
/// Сценарій використання (use case) створення замовлення.
/// Прикладний рівень оркеструє доменні об'єкти й репозиторії,
/// але не містить самих бізнес-правил — вони інкапсульовані в домені.
/// Тут реалізовано взаємодію двох bounded context через контракт
/// ICustomerRepository: перевірка існування користувача перед створенням замовлення.
/// </summary>
public sealed class CreateOrderHandler
{
    private readonly IOrderRepository _orders;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _events;
    private readonly IValidator<CreateOrderCommand> _validator;

    public CreateOrderHandler(IOrderRepository orders, ICustomerRepository customers,
        IUnitOfWork unitOfWork, IDomainEventDispatcher events, IValidator<CreateOrderCommand> validator)
    {
        _orders = orders;
        _customers = customers;
        _unitOfWork = unitOfWork;
        _events = events;
        _validator = validator;
    }

    public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default)
    {
        // Автоматична перевірка команди перед виконанням сценарію (кидає ValidationException).
        await _validator.ValidateAndThrowAsync(command, ct);

        if (!await _customers.ExistsAsync(command.CustomerId, ct))
            throw new InvalidOperationException("Користувача не знайдено.");

        var order = new Order(command.CustomerId);
        foreach (var item in command.Items)
            order.AddItem(item.ProductName, new Money(item.UnitPrice), item.Quantity);

        order.Confirm();
        await _orders.AddAsync(order, ct);

        // Межа транзакції: збереження всіх змін одним викликом (Unit of Work).
        await _unitOfWork.SaveChangesAsync(ct);

        // Публікуємо доменні події (слабка зв'язаність зі споживачами).
        await _events.DispatchAsync(order.DomainEvents, ct);
        order.ClearDomainEvents();
        return order.Id;
    }
}
