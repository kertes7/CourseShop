using CourseShop.Application.Common.Events;
using CourseShop.Domain.Ordering.Events;
using Microsoft.Extensions.Logging;

namespace CourseShop.Application.Ordering.EventHandlers;

/// <summary>
/// Другий незалежний споживач тієї ж події: фіксує замовлення в аналітиці.
/// Додавання нового обробника не потребує зміни коду ініціатора події.
/// </summary>
public sealed class OrderConfirmedAnalyticsHandler : IEventHandler<OrderConfirmedEvent>
{
    private readonly ILogger<OrderConfirmedAnalyticsHandler> _logger;

    public OrderConfirmedAnalyticsHandler(ILogger<OrderConfirmedAnalyticsHandler> logger)
        => _logger = logger;

    public Task HandleAsync(OrderConfirmedEvent e, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "[Аналітика] Зафіксовано підтверджене замовлення {OrderId}; дохід +{Total}.",
            e.OrderId, e.TotalAmount);
        return Task.CompletedTask;
    }
}
