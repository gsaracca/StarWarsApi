using StarWarsApi.Client;
using StarWarsApi.Models;

namespace StarWarsApi.ViewModels;

/// <summary>
/// Groups all results of the same SWAPI resource type (Characters, Films, etc.)
/// into a single display unit with its category metadata.
/// </summary>
public sealed class SearchResultGroup
{
    public required string                          Category    { get; init; }
    public required string                          Icon        { get; init; }
    public required int                             TotalCount  { get; init; }
    public required bool                            HasNext     { get; init; }
    public required bool                            HasPrevious { get; init; }
    public required IReadOnlyList<SearchResultItem> Items       { get; init; }

    // Computed display strings used directly in XAML bindings
    public string CategoryHeader => $"{Icon}  {Category}";
    public string PageInfo       => $"{TotalCount} result(s) found in the galaxy";

    // ── Factories ────────────────────────────────────────────────────────────

    public static SearchResultGroup FromPeople(PagedResult<Person> result) => new()
    {
        Category    = "Characters",
        Icon        = "👤",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(p => new SearchResultItem
        {
            Category          = "Characters",
            CategoryIcon      = "👤",
            CategoryBadge     = "Character",
            CategoryAccentHex = "#4FC3F7",
            Name              = p.Name,
            Subtitle          = $"{Capitalize(p.Gender)}  ·  Born {p.BirthYear}",
            Detail1           = $"Height: {p.Height} cm   Mass: {p.Mass} kg",
            Detail2           = $"Eyes: {p.EyeColor}   Hair: {p.HairColor}",
            Detail3           = $"Appears in {p.Films.Count} film(s)",
            SourceUrl         = p.Url,
            ImageUrl          = VisualGuideEndpoints.Character(VisualGuideEndpoints.ExtractId(p.Url)),
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromFilms(PagedResult<Film> result) => new()
    {
        Category    = "Films",
        Icon        = "🎬",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(f => new SearchResultItem
        {
            Category          = "Films",
            CategoryIcon      = "🎬",
            CategoryBadge     = $"Episode {f.EpisodeId}",
            CategoryAccentHex = "#FFE81F",
            Name              = f.Title,
            Subtitle          = $"Released: {f.ReleaseDate}",
            Detail1           = $"Director: {f.Director}",
            Detail2           = $"Producer: {f.Producer}",
            Detail3           = $"{f.Characters.Count} characters  ·  {f.Planets.Count} planets",
            SourceUrl         = f.Url,
            ImageUrl          = VisualGuideEndpoints.Film(VisualGuideEndpoints.ExtractId(f.Url)),
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromStarships(PagedResult<Starship> result) => new()
    {
        Category    = "Starships",
        Icon        = "🚀",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(s => new SearchResultItem
        {
            Category          = "Starships",
            CategoryIcon      = "🚀",
            CategoryBadge     = s.StarshipClass,
            CategoryAccentHex = "#66BB6A",
            Name              = s.Name,
            Subtitle          = s.Model,
            Detail1           = $"Crew: {s.Crew}   Passengers: {s.Passengers}",
            Detail2           = $"Hyperdrive: {s.HyperdriveRating}   MGLT: {s.Mglt}",
            Detail3           = $"Cost: {s.CostInCredits} credits",
            SourceUrl         = s.Url,
            ImageUrl          = VisualGuideEndpoints.Starship(VisualGuideEndpoints.ExtractId(s.Url)),
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromVehicles(PagedResult<Vehicle> result) => new()
    {
        Category    = "Vehicles",
        Icon        = "🛸",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(v => new SearchResultItem
        {
            Category          = "Vehicles",
            CategoryIcon      = "🛸",
            CategoryBadge     = v.VehicleClass,
            CategoryAccentHex = "#FFA726",
            Name              = v.Name,
            Subtitle          = v.Model,
            Detail1           = $"Crew: {v.Crew}   Passengers: {v.Passengers}",
            Detail2           = $"Max speed: {v.MaxAtmospheringSpeed} km/h",
            Detail3           = $"Cargo: {v.CargoCapacity} kg",
            SourceUrl         = v.Url,
            ImageUrl          = VisualGuideEndpoints.Vehicle(VisualGuideEndpoints.ExtractId(v.Url)),
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromSpecies(PagedResult<Species> result) => new()
    {
        Category    = "Species",
        Icon        = "👽",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(s => new SearchResultItem
        {
            Category          = "Species",
            CategoryIcon      = "👽",
            CategoryBadge     = s.Classification,
            CategoryAccentHex = "#AB47BC",
            Name              = s.Name,
            Subtitle          = $"{Capitalize(s.Designation)}  ·  Language: {s.Language}",
            Detail1           = $"Avg. height: {s.AverageHeight} cm",
            Detail2           = $"Avg. lifespan: {s.AverageLifespan} years",
            Detail3           = $"Appears in {s.Films.Count} film(s)",
            SourceUrl         = s.Url,
            ImageUrl          = VisualGuideEndpoints.Species(VisualGuideEndpoints.ExtractId(s.Url)),
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromPlanets(PagedResult<Planet> result) => new()
    {
        Category    = "Planets",
        Icon        = "🪐",
        TotalCount  = result.Count,
        HasNext     = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items       = result.Results.Select(p => new SearchResultItem
        {
            Category          = "Planets",
            CategoryIcon      = "🪐",
            CategoryBadge     = p.Climate,
            CategoryAccentHex = "#26C6DA",
            Name              = p.Name,
            Subtitle          = $"Terrain: {p.Terrain}",
            Detail1           = $"Population: {p.Population}",
            Detail2           = $"Diameter: {p.Diameter} km   Gravity: {p.Gravity}",
            Detail3           = $"Appears in {p.Films.Count} film(s)",
            SourceUrl         = p.Url,
            ImageUrl          = VisualGuideEndpoints.Planet(VisualGuideEndpoints.ExtractId(p.Url)),
        }).ToList().AsReadOnly()
    };

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string Capitalize(string value)
        => string.IsNullOrEmpty(value) ? value
           : char.ToUpperInvariant(value[0]) + value[1..];
}
