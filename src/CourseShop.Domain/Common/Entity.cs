namespace CourseShop.Domain.Common;

/// <summary>
/// Базовий клас сутності (Entity) у термінах DDD.
/// Сутність має унікальний ідентифікатор і порівнюється саме за ним,
/// а не за значеннями полів.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public override bool Equals(object? obj)
        => obj is Entity other && other.GetType() == GetType() && other.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
}
