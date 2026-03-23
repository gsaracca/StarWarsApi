using Newtonsoft.Json;

namespace StarWarsApi.Models;

public sealed class Planet
{
    [JsonProperty("name")]            public string Name           { get; init; } = string.Empty;
    [JsonProperty("rotation_period")] public string RotationPeriod { get; init; } = string.Empty;
    [JsonProperty("orbital_period")]  public string OrbitalPeriod  { get; init; } = string.Empty;
    [JsonProperty("diameter")]        public string Diameter        { get; init; } = string.Empty;
    [JsonProperty("climate")]         public string Climate         { get; init; } = string.Empty;
    [JsonProperty("gravity")]         public string Gravity         { get; init; } = string.Empty;
    [JsonProperty("terrain")]         public string Terrain         { get; init; } = string.Empty;
    [JsonProperty("surface_water")]   public string SurfaceWater    { get; init; } = string.Empty;
    [JsonProperty("population")]      public string Population      { get; init; } = string.Empty;

    [JsonProperty("residents")] public IReadOnlyList<string> Residents { get; init; } = [];
    [JsonProperty("films")]     public IReadOnlyList<string> Films     { get; init; } = [];

    [JsonProperty("url")] public string Url { get; init; } = string.Empty;
}
