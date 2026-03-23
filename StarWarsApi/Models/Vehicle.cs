using Newtonsoft.Json;

namespace StarWarsApi.Models;

public sealed class Vehicle
{
    [JsonProperty("name")]                   public string Name                 { get; init; } = string.Empty;
    [JsonProperty("model")]                  public string Model                { get; init; } = string.Empty;
    [JsonProperty("manufacturer")]           public string Manufacturer         { get; init; } = string.Empty;
    [JsonProperty("cost_in_credits")]        public string CostInCredits        { get; init; } = string.Empty;
    [JsonProperty("length")]                 public string Length               { get; init; } = string.Empty;
    [JsonProperty("max_atmosphering_speed")] public string MaxAtmospheringSpeed { get; init; } = string.Empty;
    [JsonProperty("crew")]                   public string Crew                 { get; init; } = string.Empty;
    [JsonProperty("passengers")]             public string Passengers           { get; init; } = string.Empty;
    [JsonProperty("cargo_capacity")]         public string CargoCapacity        { get; init; } = string.Empty;
    [JsonProperty("consumables")]            public string Consumables          { get; init; } = string.Empty;
    [JsonProperty("vehicle_class")]          public string VehicleClass         { get; init; } = string.Empty;

    [JsonProperty("pilots")] public IReadOnlyList<string> Pilots { get; init; } = [];
    [JsonProperty("films")]  public IReadOnlyList<string> Films  { get; init; } = [];

    [JsonProperty("url")] public string Url { get; init; } = string.Empty;
}
