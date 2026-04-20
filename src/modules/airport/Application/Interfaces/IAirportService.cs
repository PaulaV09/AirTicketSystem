// src/modules/airport/Application/Interfaces/IAirportService.cs
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.Interfaces;

public interface IAirportService
{
    Task<AirportEntity> CreateAsync(
        int ciudadId, string codigoIata, string codigoIcao,
        string nombre, string? direccion);
    Task<AirportEntity?> GetByIdAsync(int id);
    Task<AirportEntity?> GetByIataAsync(string codigoIata);
    Task<IEnumerable<AirportEntity>> GetAllAsync();
    Task<IEnumerable<AirportEntity>> GetActivosAsync();
    Task<IEnumerable<AirportEntity>> GetByCityAsync(int ciudadId);
    Task<AirportEntity> UpdateAsync(
        int id, string nombre, string? direccion);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}