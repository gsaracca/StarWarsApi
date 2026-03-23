using Newtonsoft.Json;

namespace StarWarsApi.Models;

public sealed class Species
{
    [JsonProperty("name")]             public string Name            { get; init; } = string.Empty;
    [JsonProperty("classification")]   public string Classification  { get; init; } = string.Empty;
    [JsonProperty("designation")]      public string Designation     { get; init; } = string.Empty;
    [JsonProperty("average_height")]   public string AverageHeight   { get; init; } = string.Empty;
    [JsonProperty("skin_colors")]      public string SkinColors      { get; init; } = string.Empty;
    [JsonProperty("hair_colors")]      public string HairColors      { get; init; } = string.Empty;
    [JsonProperty("eye_colors")]       public string EyeColors       { get; init; } = string.Empty;
    [JsonProperty("average_lifespan")] public string AverageLifespan { get; init; } = string.Empty;
    [JsonProperty("homeworld")]        public string? Homeworld      { get; init; }
    [JsonProperty("language")]         public string Language        { get; init; } = string.Empty;

    [JsonProperty("people")] public IReadOnlyList<string> People { get; init; } = [];
    [JsonProperty("films")]  public IReadOnlyList<string> Films  { get; init; } = [];

    [JsonProperty("url")] public string Url { get; init; } = string.Empty;
}
