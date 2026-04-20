// src/modules/aircraftmodel/Application/Interfaces/IAircraftModelService.cs
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Application.Interfaces;

public interface IAircraftModelService
{
    Task<AircraftModelEntity> CreateAsync(
        int fabricanteId, string nombre, string codigoModelo,
        int? autonomiKm, int? velocidadKmh, string? descripcion);
    Task<AircraftModelEntity?> GetByIdAsync(int id);
    Task<IEnumerable<AircraftModelEntity>> GetAllAsync();
    Task<IEnumerable<AircraftModelEntity>> GetByManufacturerAsync(int fabricanteId);
    Task<AircraftModelEntity> UpdateAsync(
        int id, string nombre, int? autonomiKm,
        int? velocidadKmh, string? descripcion);
    Task DeleteAsync(int id);
}