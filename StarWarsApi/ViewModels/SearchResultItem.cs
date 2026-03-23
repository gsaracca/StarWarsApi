namespace StarWarsApi.ViewModels;

/// <summary>
/// Flattened display model for a single SWAPI resource result.
/// All fields are pre-formatted strings ready for direct binding — no logic in the View.
/// </summary>
public sealed class SearchResultItem
{
    public required string Category          { get; init; }
    public required string CategoryIcon      { get; init; }   // e.g. "👤"
    public required string CategoryBadge     { get; init; }   // e.g. "Character"
    public required string CategoryAccentHex { get; init; }   // e.g. "#4FC3F7"

    public required string Name     { get; init; }
    public required string Subtitle { get; init; }
    public required string Detail1  { get; init; }
    public required string Detail2  { get; init; }
    public required string Detail3  { get; init; }

    public required string SourceUrl { get; init; }

    /// <summary>
    /// Image URL from starwars-visualguide.com, derived from the SWAPI resource ID.
    /// May return a 404 if the visual guide has no image for this entry;
    /// the View handles that gracefully with a fallback icon.
    /// </summary>
    public required string ImageUrl { get; init; }
}
