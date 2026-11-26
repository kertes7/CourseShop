namespace CourseShop.Domain.Common;

/// <summary>
/// Корінь агрегату. На відміну від звичайної сутності, здатний реєструвати
/// доменні події, які згодом публікуються прикладним рівнем.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
