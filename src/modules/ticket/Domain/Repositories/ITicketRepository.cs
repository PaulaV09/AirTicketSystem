// src/modules/ticket/Domain/Repositories/ITicketRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.ticket.Domain.Repositories;

public interface ITicketRepository : IRepository<TicketEntity>
{
    Task<TicketEntity?> GetByCodigoTiqueteAsync(string codigoTiquete);
    Task<TicketEntity?> GetByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IEnumerable<TicketEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<TicketEntity>> GetByVueloAsync(int vueloId);
    Task<bool> ExistsByCodigoTiqueteAsync(string codigoTiquete);
    Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId);
}