// src/modules/terminal/Infrastructure/repository/TerminalRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Infrastructure.repository;

public class TerminalRepository : BaseRepository<TerminalEntity>, ITerminalRepository
{
    public TerminalRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TerminalEntity>> GetByAeropuertoAsync(int aeropuertoId)
        => await _dbSet
            .Include(t => t.Puertas)
            .Where(t => t.AeropuertoId == aeropuertoId)
            .OrderBy(t => t.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAndAeropuertoAsync(
        string nombre, int aeropuertoId)
        => await _dbSet
            .AnyAsync(t =>
                t.Nombre.ToLower() == nombre.ToLower() &&
                t.AeropuertoId == aeropuertoId);
}