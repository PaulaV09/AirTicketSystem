// src/modules/aircraft/Application/Interfaces/IAircraftService.cs
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Application.Interfaces;

public interface IAircraftService
{
    Task<AircraftEntity> CreateAsync(
        int modeloAvionId, int aerolineaId, string matricula,
        DateOnly? fechaFabricacion, DateOnly? fechaProximoMantenimiento);
    Task<AircraftEntity?> GetByIdAsync(int id);
    Task<AircraftEntity?> GetByMatriculaAsync(string matricula);
    Task<IEnumerable<AircraftEntity>> GetAllAsync();
    Task<IEnumerable<AircraftEntity>> GetByAirlineAsync(int aerolineaId);
    Task<IEnumerable<AircraftEntity>> GetAvailableAsync();
    Task<IEnumerable<AircraftEntity>> GetWithUrgentMaintenanceAsync();
    Task<AircraftEntity> UpdateAsync(
        int id, DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento);
    Task SendToMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento);
    Task RegisterMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento);
    Task RegisterLandingAsync(int id, decimal horasVuelo);
    Task DecommissionAsync(int id);
    Task ReactivateAsync(int id);
    Task DeleteAsync(int id);
}