namespace StarWarsApi.ViewModels;

/// <summary>
/// Flattened display model for a single SWAPI resource result.
/// All fields are preformatted for direct binding in the view.
/// </summary>
public sealed class SearchResultItem
{
    public required string Category { get; init; }
    public required string CategoryIcon { get; init; }
    public required string CategoryBadge { get; init; }
    public required string CategoryAccentHex { get; init; }

    public required string Name { get; init; }
    public required string Subtitle { get; init; }
    public required string Detail1 { get; init; }
    public required string Detail2 { get; init; }
    public required string Detail3 { get; init; }

    public required string SourceUrl { get; init; }
    public required string ImageUrl { get; init; }
}
