using StarWarsApi.Client;
using StarWarsApi.Infrastructure;
using StarWarsApi.Models;
using StarWarsApi.ViewModels;

namespace StarWarsApi.Services;

/// <summary>
/// Fires all six SWAPI endpoint queries in parallel via Task.WhenAll,
/// then assembles non-empty results into ordered SearchResultGroups.
/// </summary>
public sealed class SearchService : ISearchService
{
    private readonly ISwapiClient _client;

    public SearchService(ISwapiClient client) => _client = client;

    public async Task<IReadOnlyList<SearchResultGroup>> SearchAllAsync(
        string query, int page = 1, CancellationToken ct = default)
    {
        AppLogger.Instance.Information(
            "Initiating parallel galaxy search — query={Query} page={Page}", query, page);

        // All six requests dispatched simultaneously
        var peopleTask    = _client.GetPeopleAsync   (query, page, ct);
        var filmsTask     = _client.GetFilmsAsync    (query, page, ct);
        var starshipsTask = _client.GetStarshipsAsync(query, page, ct);
        var vehiclesTask  = _client.GetVehiclesAsync (query, page, ct);
        var speciesTask   = _client.GetSpeciesAsync  (query, page, ct);
        var planetsTask   = _client.GetPlanetsAsync  (query, page, ct);

        await Task.WhenAll(peopleTask, filmsTask, starshipsTask,
                           vehiclesTask, speciesTask, planetsTask)
                  .ConfigureAwait(false);

        var groups = new List<SearchResultGroup>(6);

        AddIfNotEmpty(groups, peopleTask.Result,    SearchResultGroup.FromPeople);
        AddIfNotEmpty(groups, filmsTask.Result,     SearchResultGroup.FromFilms);
        AddIfNotEmpty(groups, starshipsTask.Result, SearchResultGroup.FromStarships);
        AddIfNotEmpty(groups, vehiclesTask.Result,  SearchResultGroup.FromVehicles);
        AddIfNotEmpty(groups, speciesTask.Result,   SearchResultGroup.FromSpecies);
        AddIfNotEmpty(groups, planetsTask.Result,   SearchResultGroup.FromPlanets);

        AppLogger.Instance.Information(
            "Search completed — {Categories} categories, {Total} total records matched",
            groups.Count,
            groups.Sum(g => g.TotalCount));

        return groups.AsReadOnly();
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static void AddIfNotEmpty<T>(
        List<SearchResultGroup> groups,
        PagedResult<T>          result,
        Func<PagedResult<T>, SearchResultGroup> factory)
    {
        if (result.Results.Count > 0)
            groups.Add(factory(result));
    }
}
