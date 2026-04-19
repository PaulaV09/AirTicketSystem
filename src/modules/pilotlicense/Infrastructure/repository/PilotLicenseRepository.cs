// src/modules/pilotlicense/Infrastructure/repository/PilotLicenseRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Infrastructure.repository;

public class PilotLicenseRepository
    : BaseRepository<PilotLicenseEntity>, IPilotLicenseRepository
{
    public PilotLicenseRepository(AppDbContext context) : base(context) { }

    public async Task<PilotLicenseEntity?> GetByNumeroLicenciaAsync(string numeroLicencia)
        => await _dbSet
            .Include(l => l.Trabajador)
                .ThenInclude(w => w.Persona)
            .FirstOrDefaultAsync(l =>
                l.NumeroLicencia == numeroLicencia.ToUpperInvariant());

    public async Task<IEnumerable<PilotLicenseEntity>> GetByTrabajadorAsync(
        int trabajadorId)
        => await _dbSet
            .Include(l => l.Habilitaciones)
                .ThenInclude(h => h.ModeloAvion)
            .Where(l => l.TrabajadorId == trabajadorId)
            .OrderByDescending(l => l.FechaVencimiento)
            .ToListAsync();

    public async Task<IEnumerable<PilotLicenseEntity>> GetVigentesAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        return await _dbSet
            .Include(l => l.Trabajador)
                .ThenInclude(w => w.Persona)
            .Where(l => l.Activa && l.FechaVencimiento >= hoy)
            .OrderBy(l => l.FechaVencimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<PilotLicenseEntity>> GetProximasAVencerAsync(
        int diasUmbral)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var umbral = hoy.AddDays(diasUmbral);

        return await _dbSet
            .Include(l => l.Trabajador)
                .ThenInclude(w => w.Persona)
            .Where(l =>
                l.Activa &&
                l.FechaVencimiento >= hoy &&
                l.FechaVencimiento <= umbral)
            .OrderBy(l => l.FechaVencimiento)
            .ToListAsync();
    }

    public async Task<bool> ExistsByNumeroLicenciaAsync(string numeroLicencia)
        => await _dbSet
            .AnyAsync(l =>
                l.NumeroLicencia == numeroLicencia.ToUpperInvariant());
}