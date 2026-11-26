namespace CourseShop.Application.Common;

/// <summary>
/// Абстракція кешу (прикладний рівень). Приховує конкретний механізм
/// (розподілений кеш Memcached/Redis чи локальний), відповідно до DIP.
/// </summary>
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
}
