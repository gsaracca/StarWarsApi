using Newtonsoft.Json;

namespace StarWarsApi.Models;

/// <summary>
/// Generic wrapper that matches the SWAPI paginated response envelope:
/// { count, next, previous, results: [...] }
/// </summary>
public sealed class PagedResult<T>
{
    [JsonProperty("count")]
    public int Count { get; init; }

    [JsonProperty("next")]
    public string? Next { get; init; }

    [JsonProperty("previous")]
    public string? Previous { get; init; }

    [JsonProperty("results")]
    public IReadOnlyList<T> Results { get; init; } = [];

    public bool HasNext     => Next     is not null;
    public bool HasPrevious => Previous is not null;

    public static PagedResult<T> Empty => new();
}
