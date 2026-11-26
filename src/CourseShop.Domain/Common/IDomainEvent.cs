namespace CourseShop.Domain.Common;

/// <summary>
/// Доменна подія — факт, що відбувся у предметній області.
/// Не містить бізнес-логіки й не знає про своїх споживачів.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
