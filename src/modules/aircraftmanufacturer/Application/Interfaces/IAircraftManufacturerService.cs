// src/modules/aircraftmanufacturer/Application/Interfaces/IAircraftManufacturerService.cs
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.Interfaces;

public interface IAircraftManufacturerService
{
    Task<AircraftManufacturerEntity> CreateAsync(
        int paisId, string nombre, string? sitioWeb);
    Task<AircraftManufacturerEntity?> GetByIdAsync(int id);
    Task<IEnumerable<AircraftManufacturerEntity>> GetAllAsync();
    Task<IEnumerable<AircraftManufacturerEntity>> GetByCountryAsync(int paisId);
    Task<AircraftManufacturerEntity> UpdateAsync(
        int id, string nombre, string? sitioWeb);
    Task DeleteAsync(int id);
}