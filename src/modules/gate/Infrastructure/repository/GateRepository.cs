// src/modules/gate/Infrastructure/repository/GateRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Infrastructure.repository;

public class GateRepository : BaseRepository<GateEntity>, IGateRepository
{
    public GateRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<GateEntity>> GetByTerminalAsync(int terminalId)
        => await _dbSet
            .Where(g => g.TerminalId == terminalId)
            .OrderBy(g => g.Codigo)
            .ToListAsync();

    public async Task<IEnumerable<GateEntity>> GetActivasByTerminalAsync(int terminalId)
        => await _dbSet
            .Where(g => g.TerminalId == terminalId && g.Activa)
            .OrderBy(g => g.Codigo)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoAndTerminalAsync(
        string codigo, int terminalId)
        => await _dbSet
            .AnyAsync(g =>
                g.Codigo == codigo.ToUpperInvariant() &&
                g.TerminalId == terminalId);
}