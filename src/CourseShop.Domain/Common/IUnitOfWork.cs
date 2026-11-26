namespace CourseShop.Domain.Common;

/// <summary>
/// Unit of Work — абстракція межі транзакції. Репозиторії лише реєструють зміни,
/// а фактичне їх збереження відбувається одним викликом SaveChangesAsync.
/// Аналог DbContext.SaveChanges у EF Core.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
