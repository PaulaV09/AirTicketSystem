// src/modules/terminal/Application/Interfaces/ITerminalService.cs
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Application.Interfaces;

public interface ITerminalService
{
    Task<TerminalEntity> CreateAsync(
        int aeropuertoId, string nombre, string? descripcion);
    Task<TerminalEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TerminalEntity>> GetByAirportAsync(int aeropuertoId);
    Task<TerminalEntity> UpdateAsync(
        int id, string nombre, string? descripcion);
    Task DeleteAsync(int id);
}