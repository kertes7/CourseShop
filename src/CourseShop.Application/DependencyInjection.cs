using CourseShop.Application.Common.Events;
using CourseShop.Application.Ordering.CreateOrder;
using CourseShop.Application.Ordering.EventHandlers;
using CourseShop.Domain.Ordering.Events;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CourseShop.Application;

/// <summary>
/// Реєстрація сервісів прикладного рівня в контейнері DI.
/// Кожен шар відповідає за реєстрацію власних залежностей (Composition Root).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Валідатори FluentValidation з цієї збірки.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Сценарії використання — у межах запиту (Scoped).
        services.AddScoped<CreateOrderHandler>();

        // Обробники подій (кілька реалізацій для однієї події).
        services.AddScoped<IEventHandler<OrderConfirmedEvent>, OrderConfirmedNotificationHandler>();
        services.AddScoped<IEventHandler<OrderConfirmedEvent>, OrderConfirmedAnalyticsHandler>();

        return services;
    }
}
