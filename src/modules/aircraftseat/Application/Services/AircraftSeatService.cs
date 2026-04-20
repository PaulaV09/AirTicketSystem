// src/modules/aircraftseat/Application/Services/AircraftSeatService.cs
using AirTicketSystem.modules.aircraftseat.Application.Interfaces;
using AirTicketSystem.modules.aircraftseat.Application.UseCases;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Application.Services;

public class AircraftSeatService : IAircraftSeatService
{
    private readonly CreateAircraftSeatUseCase _create;
    private readonly GetAircraftSeatByIdUseCase _getById;
    private readonly GetSeatsByAircraftUseCase _getByAircraft;
    private readonly GetSeatsByAircraftAndClassUseCase _getByAircraftAndClass;
    private readonly UpdateAircraftSeatUseCase _update;
    private readonly ActivateAircraftSeatUseCase _activate;
    private readonly DeactivateAircraftSeatUseCase _deactivate;
    private readonly DeleteAircraftSeatUseCase _delete;

    public AircraftSeatService(
        CreateAircraftSeatUseCase create,
        GetAircraftSeatByIdUseCase getById,
        GetSeatsByAircraftUseCase getByAircraft,
        GetSeatsByAircraftAndClassUseCase getByAircraftAndClass,
        UpdateAircraftSeatUseCase update,
        ActivateAircraftSeatUseCase activate,
        DeactivateAircraftSeatUseCase deactivate,
        DeleteAircraftSeatUseCase delete)
    {
        _create                = create;
        _getById               = getById;
        _getByAircraft         = getByAircraft;
        _getByAircraftAndClass = getByAircraftAndClass;
        _update                = update;
        _activate              = activate;
        _deactivate            = deactivate;
        _delete                = delete;
    }

    public Task<AircraftSeatEntity> CreateAsync(
        int avionId, int claseServicioId,
        int fila, char columna,
        bool esVentana, bool esPasillo,
        decimal costoSeleccion)
        => _create.ExecuteAsync(
            avionId, claseServicioId, fila, columna,
            esVentana, esPasillo, costoSeleccion);

    public Task<AircraftSeatEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<AircraftSeatEntity>> GetByAircraftAsync(int avionId)
        => _getByAircraft.ExecuteAsync(avionId);

    public Task<IEnumerable<AircraftSeatEntity>> GetByAircraftAndClassAsync(
        int avionId, int claseServicioId)
        => _getByAircraftAndClass.ExecuteAsync(avionId, claseServicioId);

    public Task<AircraftSeatEntity> UpdateAsync(
        int id, bool esVentana, bool esPasillo, decimal costoSeleccion)
        => _update.ExecuteAsync(id, esVentana, esPasillo, costoSeleccion);

    public Task ActivateAsync(int id) => _activate.ExecuteAsync(id);
    public Task DeactivateAsync(int id) => _deactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}