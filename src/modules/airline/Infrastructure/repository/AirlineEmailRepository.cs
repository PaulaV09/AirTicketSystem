// src/modules/airline/Infrastructure/repository/AirlineEmailRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public class AirlineEmailRepository
    : BaseRepository<AirlineEmailEntity>, IAirlineEmailRepository
{
    public AirlineEmailRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AirlineEmailEntity>> GetByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(e => e.TipoEmail)
            .Where(e => e.AerolineaId == aerolineaId)
            .OrderByDescending(e => e.EsPrincipal)
            .ToListAsync();

    public async Task<AirlineEmailEntity?> GetPrincipalByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(e => e.TipoEmail)
            .FirstOrDefaultAsync(e =>
                e.AerolineaId == aerolineaId && e.EsPrincipal);

    public async Task<bool> ExistsByEmailAndAerolineaAsync(
        string email, int aerolineaId)
        => await _dbSet
            .AnyAsync(e =>
                e.Email.ToLower() == email.ToLower() &&
                e.AerolineaId == aerolineaId);

    public async Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId)
    {
        var emails = await _dbSet
            .Where(e => e.AerolineaId == aerolineaId && e.EsPrincipal)
            .ToListAsync();

        foreach (var email in emails)
            email.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}