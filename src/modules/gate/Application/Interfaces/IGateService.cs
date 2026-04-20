// src/modules/gate/Application/Interfaces/IGateService.cs
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Application.Interfaces;

public interface IGateService
{
    Task<GateEntity> CreateAsync(int terminalId, string codigo);
    Task<GateEntity?> GetByIdAsync(int id);
    Task<IEnumerable<GateEntity>> GetByTerminalAsync(int terminalId);
    Task<IEnumerable<GateEntity>> GetActivasByTerminalAsync(int terminalId);
    Task<GateEntity> UpdateAsync(int id, string codigo);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}