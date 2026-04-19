// src/modules/terminal/Domain/Repositories/ITerminalRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Domain.Repositories;

public interface ITerminalRepository : IRepository<TerminalEntity>
{
    Task<IEnumerable<TerminalEntity>> GetByAeropuertoAsync(int aeropuertoId);
    Task<bool> ExistsByNombreAndAeropuertoAsync(string nombre, int aeropuertoId);
}