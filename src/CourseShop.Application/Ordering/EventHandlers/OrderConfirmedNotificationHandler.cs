using CourseShop.Application.Common.Events;
using CourseShop.Domain.Ordering.Events;
using Microsoft.Extensions.Logging;

namespace CourseShop.Application.Ordering.EventHandlers;

/// <summary>
/// Споживач події OrderConfirmed: надсилає сповіщення користувачу.
/// Не має прямої залежності від ініціатора події.
/// </summary>
public sealed class OrderConfirmedNotificationHandler : IEventHandler<OrderConfirmedEvent>
{
    private readonly ILogger<OrderConfirmedNotificationHandler> _logger;

    public OrderConfirmedNotificationHandler(ILogger<OrderConfirmedNotificationHandler> logger)
        => _logger = logger;

    public Task HandleAsync(OrderConfirmedEvent e, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "[Сповіщення] Користувачу {CustomerId} надіслано підтвердження замовлення {OrderId} на суму {Total}.",
            e.CustomerId, e.OrderId, e.TotalAmount);
        return Task.CompletedTask;
    }
}
