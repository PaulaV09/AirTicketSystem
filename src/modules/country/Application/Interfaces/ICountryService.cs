// src/modules/country/Application/Interfaces/ICountryService.cs
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Application.Interfaces;

public interface ICountryService
{
    Task<CountryEntity> CreateAsync(
        int continenteId, string nombre,
        string codigoIso2, string codigoIso3);
    Task<CountryEntity?> GetByIdAsync(int id);
    Task<IEnumerable<CountryEntity>> GetAllAsync();
    Task<IEnumerable<CountryEntity>> GetByContinentAsync(int continenteId);
    Task<CountryEntity> UpdateAsync(
        int id, string nombre,
        string codigoIso2, string codigoIso3);
    Task DeleteAsync(int id);
}