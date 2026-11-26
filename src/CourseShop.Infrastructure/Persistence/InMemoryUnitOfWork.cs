using CourseShop.Domain.Common;

namespace CourseShop.Infrastructure.Persistence;

/// <summary>Реалізація Unit of Work над InMemoryDatabase.</summary>
public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly InMemoryDatabase _db;

    public InMemoryUnitOfWork(InMemoryDatabase db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => Task.FromResult(_db.Commit());
}
