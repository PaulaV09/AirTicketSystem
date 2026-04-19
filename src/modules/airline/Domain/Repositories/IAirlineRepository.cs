// src/modules/airline/Domain/Repositories/IAirlineRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlineRepository : IRepository<AirlineEntity>
{
    Task<AirlineEntity?> GetByCodigoIataAsync(string codigoIata);
    Task<AirlineEntity?> GetByCodigoIcaoAsync(string codigoIcao);
    Task<IEnumerable<AirlineEntity>> GetActivasAsync();
    Task<bool> ExistsByCodigoIataAsync(string codigoIata);
    Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao);
}