// src/modules/airport/Domain/Repositories/IAirportRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Domain.Repositories;

public interface IAirportRepository : IRepository<AirportEntity>
{
    Task<AirportEntity?> GetByCodigoIataAsync(string codigoIata);
    Task<AirportEntity?> GetByCodigoIcaoAsync(string codigoIcao);
    Task<IEnumerable<AirportEntity>> GetByCiudadAsync(int ciudadId);
    Task<IEnumerable<AirportEntity>> GetActivosAsync();
    Task<bool> ExistsByCodigoIataAsync(string codigoIata);
    Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao);
}