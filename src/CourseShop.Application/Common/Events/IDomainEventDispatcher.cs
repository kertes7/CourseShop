using CourseShop.Domain.Common;

namespace CourseShop.Application.Common.Events;

/// <summary>
/// Диспетчер доменних подій. Приховує механізм доставки події її обробникам,
/// забезпечуючи слабку зв'язаність між ініціатором (producer) і споживачами (consumer).
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default);
}
