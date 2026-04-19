// src/modules/flighthistory/Domain/Repositories/IFlightHistoryRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.flighthistory.Infrastructure.entity;

namespace AirTicketSystem.modules.flighthistory.Domain.Repositories;

public interface IFlightHistoryRepository : IRepository<FlightHistoryEntity>
{
    Task<IEnumerable<FlightHistoryEntity>> GetByVueloAsync(int vueloId);
    Task<FlightHistoryEntity?> GetUltimoCambioByVueloAsync(int vueloId);
}