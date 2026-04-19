// src/modules/airline/Infrastructure/repository/AirlinePhoneRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public class AirlinePhoneRepository
    : BaseRepository<AirlinePhoneEntity>, IAirlinePhoneRepository
{
    public AirlinePhoneRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AirlinePhoneEntity>> GetByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(p => p.TipoTelefono)
            .Where(p => p.AerolineaId == aerolineaId)
            .OrderByDescending(p => p.EsPrincipal)
            .ToListAsync();

    public async Task<AirlinePhoneEntity?> GetPrincipalByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(p => p.TipoTelefono)
            .FirstOrDefaultAsync(p =>
                p.AerolineaId == aerolineaId && p.EsPrincipal);

    public async Task<bool> ExistsByNumeroAndAerolineaAsync(
        string numero, int aerolineaId)
        => await _dbSet
            .AnyAsync(p =>
                p.Numero == numero &&
                p.AerolineaId == aerolineaId);

    public async Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId)
    {
        var telefonos = await _dbSet
            .Where(p => p.AerolineaId == aerolineaId && p.EsPrincipal)
            .ToListAsync();

        foreach (var telefono in telefonos)
            telefono.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}