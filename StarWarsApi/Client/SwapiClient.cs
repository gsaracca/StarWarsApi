using Newtonsoft.Json;
using StarWarsApi.Infrastructure;
using StarWarsApi.Models;
using StarWarsApi.Services;

namespace StarWarsApi.Client;

/// <summary>
/// HTTP client for swapi.dev.
/// Every request is transparently cached; cache keys encode endpoint + query + page.
/// All HTTP activity and cache decisions are logged via AppLogger.
/// </summary>
public sealed class SwapiClient : ISwapiClient
{
    private readonly HttpClient    _http;
    private readonly ICacheService _cache;

    public SwapiClient(HttpClient http, ICacheService cache)
    {
        _http  = http;
        _cache = cache;
    }

    // ── Public API ──────────────────────────────────────────────────────────

    public Task<PagedResult<Person>>   GetPeopleAsync   (string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Person>  (SwapiEndpoints.People,    search, page, ct);

    public Task<PagedResult<Film>>     GetFilmsAsync    (string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Film>    (SwapiEndpoints.Films,     search, page, ct);

    public Task<PagedResult<Starship>> GetStarshipsAsync(string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Starship>(SwapiEndpoints.Starships, search, page, ct);

    public Task<PagedResult<Vehicle>>  GetVehiclesAsync (string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Vehicle> (SwapiEndpoints.Vehicles,  search, page, ct);

    public Task<PagedResult<Species>>  GetSpeciesAsync  (string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Species> (SwapiEndpoints.Species,   search, page, ct);

    public Task<PagedResult<Planet>>   GetPlanetsAsync  (string search, int page = 1, CancellationToken ct = default)
        => FetchAsync<Planet>  (SwapiEndpoints.Planets,   search, page, ct);

    // ── Private infrastructure ───────────────────────────────────────────────

    private async Task<PagedResult<T>> FetchAsync<T>(
        string endpoint, string search, int page, CancellationToken ct)
    {
        var cacheKey = BuildCacheKey(endpoint, search, page);

        if (_cache.TryGet<PagedResult<T>>(cacheKey, out var cached))
            return cached!;

        var url = string.IsNullOrWhiteSpace(search)
            ? SwapiEndpoints.WithPage(endpoint, page)
            : SwapiEndpoints.WithSearch(endpoint, search, page);

        AppLogger.Instance.Information("HTTP GET {Url}", url);

        var response = await _http.GetAsync(url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var json   = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var result = JsonConvert.DeserializeObject<PagedResult<T>>(json) ?? PagedResult<T>.Empty;

        AppLogger.Instance.Information(
            "Fetched {Count}/{Total} {Resource} record(s) (page {Page})",
            result.Results.Count, result.Count, typeof(T).Name, page);

        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return result;
    }

    private static string BuildCacheKey(string endpoint, string search, int page)
        => $"{endpoint}|{search.Trim().ToLowerInvariant()}|{page}";
}
