using StarWarsApi.Models;

namespace StarWarsApi.Client;

/// <summary>
/// Contract for the SWAPI HTTP client.
/// Each method maps 1-to-1 with one SWAPI resource endpoint.
/// </summary>
public interface ISwapiClient
{
    Task<PagedResult<Person>>   GetPeopleAsync   (string search, int page = 1, CancellationToken ct = default);
    Task<PagedResult<Film>>     GetFilmsAsync    (string search, int page = 1, CancellationToken ct = default);
    Task<PagedResult<Starship>> GetStarshipsAsync(string search, int page = 1, CancellationToken ct = default);
    Task<PagedResult<Vehicle>>  GetVehiclesAsync (string search, int page = 1, CancellationToken ct = default);
    Task<PagedResult<Species>>  GetSpeciesAsync  (string search, int page = 1, CancellationToken ct = default);
    Task<PagedResult<Planet>>   GetPlanetsAsync  (string search, int page = 1, CancellationToken ct = default);
}
