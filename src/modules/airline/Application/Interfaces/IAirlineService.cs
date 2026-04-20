// src/modules/airline/Application/Interfaces/IAirlineService.cs
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Application.Interfaces;

public interface IAirlineService
{
    Task<AirlineEntity> CreateAsync(
        int paisId, string codigoIata, string codigoIcao,
        string nombre, string? nombreComercial, string? sitioWeb);
    Task<AirlineEntity?> GetByIdAsync(int id);
    Task<AirlineEntity?> GetByIataAsync(string codigoIata);
    Task<IEnumerable<AirlineEntity>> GetAllAsync();
    Task<IEnumerable<AirlineEntity>> GetActivasAsync();
    Task<AirlineEntity> UpdateAsync(
        int id, string nombre, string? nombreComercial, string? sitioWeb);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
    Task<AirlinePhoneEntity> AddPhoneAsync(
        int aerolineaId, int tipoTelefonoId,
        string numero, string? indicativo, bool esPrincipal);
    Task RemovePhoneAsync(int phoneId);
    Task<AirlineEmailEntity> AddEmailAsync(
        int aerolineaId, int tipoEmailId,
        string email, bool esPrincipal);
    Task RemoveEmailAsync(int emailId);
}