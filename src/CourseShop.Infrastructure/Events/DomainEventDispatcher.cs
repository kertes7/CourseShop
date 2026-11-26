using CourseShop.Application.Common.Events;
using CourseShop.Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace CourseShop.Infrastructure.Events;

/// <summary>
/// Реалізація диспетчера доменних подій. Для кожної події знаходить усіх
/// зареєстрованих обробників IEventHandler&lt;TEvent&gt; і викликає їх.
/// Ініціатор події не знає про конкретні обробники — зв'язаність слабка.
/// </summary>
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _provider;

    public DomainEventDispatcher(IServiceProvider provider) => _provider = provider;

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _provider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null) continue;
                var method = handlerType.GetMethod(nameof(IEventHandler<IDomainEvent>.HandleAsync))!;
                await (Task)method.Invoke(handler, new object[] { domainEvent, ct })!;
            }
        }
    }
}
