using Newtonsoft.Json;

namespace StarWarsApi.Models;

public sealed class Person
{
    [JsonProperty("name")]       public string Name      { get; init; } = string.Empty;
    [JsonProperty("height")]     public string Height    { get; init; } = string.Empty;
    [JsonProperty("mass")]       public string Mass      { get; init; } = string.Empty;
    [JsonProperty("hair_color")] public string HairColor { get; init; } = string.Empty;
    [JsonProperty("skin_color")] public string SkinColor { get; init; } = string.Empty;
    [JsonProperty("eye_color")]  public string EyeColor  { get; init; } = string.Empty;
    [JsonProperty("birth_year")] public string BirthYear { get; init; } = string.Empty;
    [JsonProperty("gender")]     public string Gender    { get; init; } = string.Empty;
    [JsonProperty("homeworld")]  public string Homeworld { get; init; } = string.Empty;

    [JsonProperty("films")]     public IReadOnlyList<string> Films     { get; init; } = [];
    [JsonProperty("species")]   public IReadOnlyList<string> Species   { get; init; } = [];
    [JsonProperty("vehicles")]  public IReadOnlyList<string> Vehicles  { get; init; } = [];
    [JsonProperty("starships")] public IReadOnlyList<string> Starships { get; init; } = [];

    [JsonProperty("url")] public string Url { get; init; } = string.Empty;
}
