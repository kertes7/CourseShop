namespace CourseShop.Application.Common;

/// <summary>
/// Абстракція для отримання демонстраційних даних.
/// Вводиться для усунення порушення DIP: вебшар має залежати від абстракції,
/// а не від конкретного інфраструктурного класу (InMemoryCustomerRepository).
/// </summary>
public interface IDemoDataProvider
{
    Guid GetDemoCustomerId();
}
