using StarWarsApi.ViewModels;

namespace StarWarsApi.Services;

/// <summary>
/// Orchestrates a simultaneous search across all six SWAPI resource types
/// and returns the results grouped by category, ready for display.
/// </summary>
public interface ISearchService
{
    Task<IReadOnlyList<SearchResultGroup>> SearchAllAsync(
        string query, int page = 1, CancellationToken ct = default);
}
