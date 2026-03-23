namespace StarWarsApi.Client;

/// <summary>
/// All SWAPI endpoint paths and URL-building helpers.
/// Keeps every URL construction in one auditable place.
/// </summary>
internal static class SwapiEndpoints
{
    public const string BaseUrl   = "https://swapi.dev/api/";

    public const string People    = "people/";
    public const string Films     = "films/";
    public const string Starships = "starships/";
    public const string Vehicles  = "vehicles/";
    public const string Species   = "species/";
    public const string Planets   = "planets/";

    /// <summary>Returns a search URL: endpoint?search={query}&amp;page={page}</summary>
    public static string WithSearch(string endpoint, string query, int page = 1)
        => $"{endpoint}?search={Uri.EscapeDataString(query)}&page={page}";

    /// <summary>Returns a paginated URL without a search filter: endpoint?page={page}</summary>
    public static string WithPage(string endpoint, int page)
        => $"{endpoint}?page={page}";
}
