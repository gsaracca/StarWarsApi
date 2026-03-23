namespace StarWarsApi.Client;

/// <summary>
/// Resolves image URLs for SWAPI resources.
///
/// Characters: github.com/vieraboschkova/swapi-gallery
///   — 87 JPGs indexed by SWAPI person ID.  Verified working as of 2026-03.
///
/// Films / Planets / Starships / Vehicles / Species:
///   — No public CDN with SWAPI-compatible IDs currently available.
///     The previously popular starwars-visualguide.com domain expired and
///     was redirected to an unrelated site (2025).
///     These methods return string.Empty → the View shows an emoji fallback.
/// </summary>
internal static class VisualGuideEndpoints
{
    private const string CharacterBase =
        "https://raw.githubusercontent.com/vieraboschkova/swapi-gallery/main/static/assets/img/people";

    public static string Character(int id) => id > 0 ? $"{CharacterBase}/{id}.jpg" : string.Empty;

    public static string Film     (int id) => string.Empty;
    public static string Planet   (int id) => string.Empty;
    public static string Starship (int id) => string.Empty;
    public static string Vehicle  (int id) => string.Empty;
    public static string Species  (int id) => string.Empty;

    /// <summary>
    /// Extracts the numeric ID from a SWAPI resource URL.
    /// <example>"https://swapi.dev/api/people/1/" → 1</example>
    /// </summary>
    public static int ExtractId(string swapiUrl)
    {
        var span      = swapiUrl.AsSpan().TrimEnd('/');
        var lastSlash = span.LastIndexOf('/');

        return lastSlash >= 0 && int.TryParse(span[(lastSlash + 1)..], out var id)
            ? id
            : 0;
    }
}
