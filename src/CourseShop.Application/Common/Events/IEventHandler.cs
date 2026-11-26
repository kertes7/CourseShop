using CourseShop.Domain.Common;

namespace CourseShop.Application.Common.Events;

/// <summary>Обробник доменної події певного типу (consumer).</summary>
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
}
