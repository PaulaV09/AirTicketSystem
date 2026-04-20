// src/modules/airport/Application/Services/AirportService.cs
using AirTicketSystem.modules.airport.Application.Interfaces;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.Services;

public class AirportService : IAirportService
{
    private readonly CreateAirportUseCase _create;
    private readonly GetAirportByIdUseCase _getById;
    private readonly GetAirportByIataUseCase _getByIata;
    private readonly GetAllAirportsUseCase _getAll;
    private readonly GetActiveAirportsUseCase _getActivos;
    private readonly GetAirportsByCityUseCase _getByCity;
    private readonly UpdateAirportUseCase _update;
    private readonly ActivateAirportUseCase _activate;
    private readonly DeactivateAirportUseCase _deactivate;
    private readonly DeleteAirportUseCase _delete;

    public AirportService(
        CreateAirportUseCase create,
        GetAirportByIdUseCase getById,
        GetAirportByIataUseCase getByIata,
        GetAllAirportsUseCase getAll,
        GetActiveAirportsUseCase getActivos,
        GetAirportsByCityUseCase getByCity,
        UpdateAirportUseCase update,
        ActivateAirportUseCase activate,
        DeactivateAirportUseCase deactivate,
        DeleteAirportUseCase delete)
    {
        _create     = create;
        _getById    = getById;
        _getByIata  = getByIata;
        _getAll     = getAll;
        _getActivos = getActivos;
        _getByCity  = getByCity;
        _update     = update;
        _activate   = activate;
        _deactivate = deactivate;
        _delete     = delete;
    }

    public Task<AirportEntity> CreateAsync(
        int ciudadId, string codigoIata, string codigoIcao,
        string nombre, string? direccion)
        => _create.ExecuteAsync(ciudadId, codigoIata, codigoIcao, nombre, direccion);

    public Task<AirportEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<AirportEntity?> GetByIataAsync(string codigoIata)
        => _getByIata.ExecuteAsync(codigoIata)!;

    public Task<IEnumerable<AirportEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<AirportEntity>> GetActivosAsync()
        => _getActivos.ExecuteAsync();

    public Task<IEnumerable<AirportEntity>> GetByCityAsync(int ciudadId)
        => _getByCity.ExecuteAsync(ciudadId);

    public Task<AirportEntity> UpdateAsync(int id, string nombre, string? direccion)
        => _update.ExecuteAsync(id, nombre, direccion);

    public Task ActivateAsync(int id)
        => _activate.ExecuteAsync(id);

    public Task DeactivateAsync(int id)
        => _deactivate.ExecuteAsync(id);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}