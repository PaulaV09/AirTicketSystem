// src/modules/aircraftseat/Application/Interfaces/IAircraftSeatService.cs
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Application.Interfaces;

public interface IAircraftSeatService
{
    Task<AircraftSeatEntity> CreateAsync(
        int avionId, int claseServicioId,
        int fila, char columna,
        bool esVentana, bool esPasillo,
        decimal costoSeleccion);
    Task<AircraftSeatEntity?> GetByIdAsync(int id);
    Task<IEnumerable<AircraftSeatEntity>> GetByAircraftAsync(int avionId);
    Task<IEnumerable<AircraftSeatEntity>> GetByAircraftAndClassAsync(
        int avionId, int claseServicioId);
    Task<AircraftSeatEntity> UpdateAsync(
        int id, bool esVentana, bool esPasillo, decimal costoSeleccion);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}