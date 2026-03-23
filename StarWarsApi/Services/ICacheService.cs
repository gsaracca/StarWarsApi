namespace StarWarsApi.Services;

/// <summary>
/// Typed in-memory cache contract.
/// Decouples the caching implementation from its consumers.
/// </summary>
public interface ICacheService
{
    bool TryGet<T>(string key, out T? value);
    void Set<T>(string key, T value, TimeSpan expiration);
    void Remove(string key);
    void Clear();
}
