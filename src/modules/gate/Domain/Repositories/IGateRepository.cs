// src/modules/gate/Domain/Repositories/IGateRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Domain.Repositories;

public interface IGateRepository : IRepository<GateEntity>
{
    Task<IEnumerable<GateEntity>> GetByTerminalAsync(int terminalId);
    Task<IEnumerable<GateEntity>> GetActivasByTerminalAsync(int terminalId);
    Task<bool> ExistsByCodigoAndTerminalAsync(string codigo, int terminalId);
}