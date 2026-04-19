// src/modules/aircraftseat/Infrastructure/repository/AircraftSeatRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Infrastructure.repository;

public class AircraftSeatRepository
    : BaseRepository<AircraftSeatEntity>, IAircraftSeatRepository
{
    public AircraftSeatRepository(AppDbContext context) : base(context) { }

    public async Task<AircraftSeatEntity?> GetByCodigoAndAvionAsync(
        string codigoAsiento, int avionId)
        => await _dbSet
            .Include(a => a.ClaseServicio)
            .FirstOrDefaultAsync(a =>
                a.CodigoAsiento == codigoAsiento.ToUpperInvariant() &&
                a.AvionId == avionId);

    public async Task<IEnumerable<AircraftSeatEntity>> GetByAvionAsync(int avionId)
        => await _dbSet
            .Include(a => a.ClaseServicio)
            .Where(a => a.AvionId == avionId && a.Activo)
            .OrderBy(a => a.Fila)
            .ThenBy(a => a.Columna)
            .ToListAsync();

    public async Task<IEnumerable<AircraftSeatEntity>> GetByAvionAndClaseAsync(
        int avionId, int claseServicioId)
        => await _dbSet
            .Include(a => a.ClaseServicio)
            .Where(a =>
                a.AvionId == avionId &&
                a.ClaseServicioId == claseServicioId &&
                a.Activo)
            .OrderBy(a => a.Fila)
            .ThenBy(a => a.Columna)
            .ToListAsync();

    public async Task<int> ContarAsientosByAvionAsync(int avionId)
        => await _dbSet
            .CountAsync(a => a.AvionId == avionId && a.Activo);

    public async Task<bool> ExistsByCodigoAndAvionAsync(
        string codigoAsiento, int avionId)
        => await _dbSet
            .AnyAsync(a =>
                a.CodigoAsiento == codigoAsiento.ToUpperInvariant() &&
                a.AvionId == avionId);
}