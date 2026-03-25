using StarWarsApi.Client;
using StarWarsApi.Models;

namespace StarWarsApi.ViewModels;

/// <summary>
/// Groups all results of the same SWAPI resource type into a single display unit.
/// </summary>
public sealed class SearchResultGroup
{
    private const string CharacterIcon = "\U0001F464";
    private const string FilmIcon = "\U0001F3AC";
    private const string StarshipIcon = "\U0001F680";
    private const string VehicleIcon = "\U0001F6F8";
    private const string SpeciesIcon = "\U0001F47D";
    private const string PlanetIcon = "\U0001FA90";

    public required string Category { get; init; }
    public required string Icon { get; init; }
    public required int TotalCount { get; init; }
    public required bool HasNext { get; init; }
    public required bool HasPrevious { get; init; }
    public required IReadOnlyList<SearchResultItem> Items { get; init; }

    public string CategoryHeader => $"{Icon}  {Category}";
    public string PageInfo => $"{TotalCount} results found";

    public static SearchResultGroup FromPeople(PagedResult<Person> result) => new()
    {
        Category = "Characters",
        Icon = CharacterIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(p => new SearchResultItem
        {
            Category = "Characters",
            CategoryIcon = CharacterIcon,
            CategoryBadge = "Character",
            CategoryAccentHex = "#4FC3F7",
            Name = p.Name,
            Subtitle = $"{Capitalize(p.Gender)} | Born {p.BirthYear}",
            Detail1 = $"Height: {p.Height} cm | Mass: {p.Mass} kg",
            Detail2 = $"Eyes: {p.EyeColor} | Hair: {p.HairColor}",
            Detail3 = $"Appears in {p.Films.Count} film(s)",
            SourceUrl = p.Url,
            ImageUrl = VisualGuideEndpoints.Character(VisualGuideEndpoints.ExtractId(p.Url))
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromFilms(PagedResult<Film> result) => new()
    {
        Category = "Films",
        Icon = FilmIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(f => new SearchResultItem
        {
            Category = "Films",
            CategoryIcon = FilmIcon,
            CategoryBadge = $"Episode {f.EpisodeId}",
            CategoryAccentHex = "#FFE81F",
            Name = f.Title,
            Subtitle = $"Released: {f.ReleaseDate}",
            Detail1 = $"Director: {f.Director}",
            Detail2 = $"Producer: {f.Producer}",
            Detail3 = $"{f.Characters.Count} characters | {f.Planets.Count} planets",
            SourceUrl = f.Url,
            ImageUrl = VisualGuideEndpoints.Film(VisualGuideEndpoints.ExtractId(f.Url))
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromStarships(PagedResult<Starship> result) => new()
    {
        Category = "Starships",
        Icon = StarshipIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(s => new SearchResultItem
        {
            Category = "Starships",
            CategoryIcon = StarshipIcon,
            CategoryBadge = s.StarshipClass,
            CategoryAccentHex = "#66BB6A",
            Name = s.Name,
            Subtitle = s.Model,
            Detail1 = $"Crew: {s.Crew} | Passengers: {s.Passengers}",
            Detail2 = $"Hyperdrive: {s.HyperdriveRating} | MGLT: {s.Mglt}",
            Detail3 = $"Cost: {s.CostInCredits} credits",
            SourceUrl = s.Url,
            ImageUrl = VisualGuideEndpoints.Starship(VisualGuideEndpoints.ExtractId(s.Url))
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromVehicles(PagedResult<Vehicle> result) => new()
    {
        Category = "Vehicles",
        Icon = VehicleIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(v => new SearchResultItem
        {
            Category = "Vehicles",
            CategoryIcon = VehicleIcon,
            CategoryBadge = v.VehicleClass,
            CategoryAccentHex = "#FFA726",
            Name = v.Name,
            Subtitle = v.Model,
            Detail1 = $"Crew: {v.Crew} | Passengers: {v.Passengers}",
            Detail2 = $"Max speed: {v.MaxAtmospheringSpeed} km/h",
            Detail3 = $"Cargo: {v.CargoCapacity} kg",
            SourceUrl = v.Url,
            ImageUrl = VisualGuideEndpoints.Vehicle(VisualGuideEndpoints.ExtractId(v.Url))
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromSpecies(PagedResult<Species> result) => new()
    {
        Category = "Species",
        Icon = SpeciesIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(s => new SearchResultItem
        {
            Category = "Species",
            CategoryIcon = SpeciesIcon,
            CategoryBadge = s.Classification,
            CategoryAccentHex = "#AB47BC",
            Name = s.Name,
            Subtitle = $"{Capitalize(s.Designation)} | Language: {s.Language}",
            Detail1 = $"Avg. height: {s.AverageHeight} cm",
            Detail2 = $"Avg. lifespan: {s.AverageLifespan} years",
            Detail3 = $"Appears in {s.Films.Count} film(s)",
            SourceUrl = s.Url,
            ImageUrl = VisualGuideEndpoints.Species(VisualGuideEndpoints.ExtractId(s.Url))
        }).ToList().AsReadOnly()
    };

    public static SearchResultGroup FromPlanets(PagedResult<Planet> result) => new()
    {
        Category = "Planets",
        Icon = PlanetIcon,
        TotalCount = result.Count,
        HasNext = result.HasNext,
        HasPrevious = result.HasPrevious,
        Items = result.Results.Select(p => new SearchResultItem
        {
            Category = "Planets",
            CategoryIcon = PlanetIcon,
            CategoryBadge = p.Climate,
            CategoryAccentHex = "#26C6DA",
            Name = p.Name,
            Subtitle = $"Terrain: {p.Terrain}",
            Detail1 = $"Population: {p.Population}",
            Detail2 = $"Diameter: {p.Diameter} km | Gravity: {p.Gravity}",
            Detail3 = $"Appears in {p.Films.Count} film(s)",
            SourceUrl = p.Url,
            ImageUrl = VisualGuideEndpoints.Planet(VisualGuideEndpoints.ExtractId(p.Url))
        }).ToList().AsReadOnly()
    };

    private static string Capitalize(string value)
        => string.IsNullOrEmpty(value)
            ? value
            : char.ToUpperInvariant(value[0]) + value[1..];
}
