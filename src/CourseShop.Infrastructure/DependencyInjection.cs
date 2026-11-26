using CourseShop.Application.Common;
using CourseShop.Application.Common.Events;
using CourseShop.Domain.Common;
using CourseShop.Domain.Customers;
using CourseShop.Domain.Ordering;
using CourseShop.Infrastructure.Caching;
using CourseShop.Infrastructure.Events;
using CourseShop.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CourseShop.Infrastructure;

/// <summary>
/// Реєстрація інфраструктурних реалізацій. Абстракції оголошені в домені/додатку,
/// а конкретні класи підключаються саме тут — відповідно до DIP.
/// Життєвий цикл сховищ у пам'яті — Singleton (єдиний стан на застосунок).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryCustomerRepository>();
        services.AddSingleton<ICustomerRepository>(sp => sp.GetRequiredService<InMemoryCustomerRepository>());
        services.AddSingleton<IDemoDataProvider>(sp => sp.GetRequiredService<InMemoryCustomerRepository>());
        // Спільна "база даних" у пам'яті + репозиторій і Unit of Work над нею.
        services.AddSingleton<InMemoryDatabase>();
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        // Кешування (ПЗ9): distributed-кеш за абстракцією ICacheService.
        // Локально — distributed-memory; у продакшені замінюється на Memcached
        // (services.AddEnyimMemcached(...)) без змін коду застосунку.
        services.AddDistributedMemoryCache();
        services.AddScoped<ICacheService, DistributedCacheService>();

        return services;
    }
}
