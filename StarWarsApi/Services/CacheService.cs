using Microsoft.Extensions.Caching.Memory;
using StarWarsApi.Infrastructure;

namespace StarWarsApi.Services;

/// <summary>
/// IMemoryCache-backed implementation with structured logging on every cache operation.
/// TTL and size limits are configured here to protect memory under heavy usage.
/// </summary>
public sealed class CacheService : ICacheService, IDisposable
{
    private readonly IMemoryCache _cache;

    public CacheService()
    {
        _cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1_000   // max 1000 cached entries
        });
    }

    public bool TryGet<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out T? hit))
        {
            AppLogger.Instance.Debug("Cache HIT  key={Key}", key);
            value = hit;
            return true;
        }

        AppLogger.Instance.Debug("Cache MISS key={Key}", key);
        value = default;
        return false;
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration,
            Size = 1
        };

        _cache.Set(key, value, options);
        AppLogger.Instance.Debug("Cache SET  key={Key} ttl={Ttl}", key, expiration);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        AppLogger.Instance.Debug("Cache DEL  key={Key}", key);
    }

    public void Clear()
    {
        if (_cache is MemoryCache mc)
            mc.Compact(1.0);

        AppLogger.Instance.Information("Cache cleared (full compact)");
    }

    public void Dispose() => _cache.Dispose();
}
