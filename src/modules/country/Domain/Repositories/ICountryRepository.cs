// src/modules/country/Domain/Repositories/ICountryRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Domain.Repositories;

public interface ICountryRepository : IRepository<CountryEntity>
{
    Task<CountryEntity?> GetByCodigoIso2Async(string codigoIso2);
    Task<CountryEntity?> GetByCodigoIso3Async(string codigoIso3);
    Task<IEnumerable<CountryEntity>> GetByContinenteAsync(int continenteId);
    Task<bool> ExistsByCodigoIso2Async(string codigoIso2);
    Task<bool> ExistsByCodigoIso3Async(string codigoIso3);
}