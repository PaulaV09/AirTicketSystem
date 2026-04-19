// src/modules/seatavailability/Infrastructure/repository/SeatAvailabilityRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.repository;

public class SeatAvailabilityRepository
    : BaseRepository<SeatAvailabilityEntity>, ISeatAvailabilityRepository
{
    public SeatAvailabilityRepository(AppDbContext context) : base(context) { }

    public async Task<SeatAvailabilityEntity?> GetByVueloAndAsientoAsync(
        int vueloId, int asientoId)
        => await _dbSet
            .Include(sa => sa.Asiento)
            .FirstOrDefaultAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId);

    public async Task<IEnumerable<SeatAvailabilityEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa => sa.VueloId == vueloId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

    public async Task<IEnumerable<SeatAvailabilityEntity>> GetDisponiblesByVueloAsync(
        int vueloId)
        => await _dbSet
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE")
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

    public async Task<IEnumerable<SeatAvailabilityEntity>> GetDisponiblesByVueloAndClaseAsync(
        int vueloId, int claseServicioId)
        => await _dbSet
            .Include(sa => sa.Asiento)
            .Where(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE" &&
                sa.Asiento.ClaseServicioId == claseServicioId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

    public async Task<int> ContarDisponiblesByVueloAsync(int vueloId)
        => await _dbSet
            .CountAsync(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE");

    public async Task<bool> AsientoDisponibleAsync(int vueloId, int asientoId)
        => await _dbSet
            .AnyAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId &&
                sa.Estado == "DISPONIBLE");

    public async Task CrearDisponibilidadParaVueloAsync(int vueloId, int avionId)
    {
        var asientos = await _context.AsientosAvion
            .Where(a => a.AvionId == avionId && a.Activo)
            .ToListAsync();

        var disponibilidades = asientos.Select(a => new SeatAvailabilityEntity
        {
            VueloId   = vueloId,
            AsientoId = a.Id,
            Estado    = "DISPONIBLE"
        });

        await _context.DisponibilidadAsientos.AddRangeAsync(disponibilidades);
        await _context.SaveChangesAsync();
    }
}