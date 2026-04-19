// src/modules/pilotrating/Infrastructure/repository/PilotRatingRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Infrastructure.repository;

public class PilotRatingRepository
    : BaseRepository<PilotRatingEntity>, IPilotRatingRepository
{
    public PilotRatingRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PilotRatingEntity>> GetByLicenciaAsync(int licenciaId)
        => await _dbSet
            .Include(h => h.ModeloAvion)
            .Where(h => h.LicenciaId == licenciaId)
            .OrderBy(h => h.ModeloAvion.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<PilotRatingEntity>> GetByModeloAvionAsync(
        int modeloAvionId)
        => await _dbSet
            .Include(h => h.Licencia)
                .ThenInclude(l => l.Trabajador)
                    .ThenInclude(w => w.Persona)
            .Where(h => h.ModeloAvionId == modeloAvionId)
            .ToListAsync();

    public async Task<PilotRatingEntity?> GetByLicenciaAndModeloAsync(
        int licenciaId, int modeloAvionId)
        => await _dbSet
            .FirstOrDefaultAsync(h =>
                h.LicenciaId == licenciaId &&
                h.ModeloAvionId == modeloAvionId);

    public async Task<bool> ExistsByLicenciaAndModeloAsync(
        int licenciaId, int modeloAvionId)
        => await _dbSet
            .AnyAsync(h =>
                h.LicenciaId == licenciaId &&
                h.ModeloAvionId == modeloAvionId);

    public async Task<IEnumerable<PilotRatingEntity>> GetVigentesAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        return await _dbSet
            .Include(h => h.ModeloAvion)
            .Include(h => h.Licencia)
                .ThenInclude(l => l.Trabajador)
                    .ThenInclude(w => w.Persona)
            .Where(h => h.FechaVencimiento >= hoy)
            .OrderBy(h => h.FechaVencimiento)
            .ToListAsync();
    }
}