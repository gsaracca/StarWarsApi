using Newtonsoft.Json;

namespace StarWarsApi.Models;

public sealed class Film
{
    [JsonProperty("title")]         public string Title        { get; init; } = string.Empty;
    [JsonProperty("episode_id")]    public int    EpisodeId    { get; init; }
    [JsonProperty("opening_crawl")] public string OpeningCrawl { get; init; } = string.Empty;
    [JsonProperty("director")]      public string Director     { get; init; } = string.Empty;
    [JsonProperty("producer")]      public string Producer     { get; init; } = string.Empty;
    [JsonProperty("release_date")]  public string ReleaseDate  { get; init; } = string.Empty;

    [JsonProperty("characters")] public IReadOnlyList<string> Characters { get; init; } = [];
    [JsonProperty("planets")]    public IReadOnlyList<string> Planets    { get; init; } = [];
    [JsonProperty("starships")]  public IReadOnlyList<string> Starships  { get; init; } = [];
    [JsonProperty("vehicles")]   public IReadOnlyList<string> Vehicles   { get; init; } = [];
    [JsonProperty("species")]    public IReadOnlyList<string> Species    { get; init; } = [];

    [JsonProperty("url")] public string Url { get; init; } = string.Empty;
}
