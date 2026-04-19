// src/modules/fare/Infrastructure/repository/FareRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.fare.Infrastructure.entity;

namespace AirTicketSystem.modules.fare.Infrastructure.repository;

public class FareRepository : BaseRepository<FareEntity>, IFareRepository
{
    public FareRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<FareEntity>> GetByRutaAsync(int rutaId)
        => await _dbSet
            .Include(f => f.ClaseServicio)
            .Include(f => f.RestriccionesEquipaje)
                .ThenInclude(r => r.TipoEquipaje)
            .Where(f => f.RutaId == rutaId)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();

    public async Task<IEnumerable<FareEntity>> GetByRutaAndClaseAsync(
        int rutaId, int claseServicioId)
        => await _dbSet
            .Include(f => f.ClaseServicio)
            .Where(f =>
                f.RutaId == rutaId &&
                f.ClaseServicioId == claseServicioId &&
                f.Activa)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();

    public async Task<IEnumerable<FareEntity>> GetActivasAsync()
        => await _dbSet
            .Include(f => f.Ruta)
            .Include(f => f.ClaseServicio)
            .Where(f => f.Activa)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();

    public async Task<IEnumerable<FareEntity>> GetActivasByRutaAsync(int rutaId)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        return await _dbSet
            .Include(f => f.ClaseServicio)
            .Where(f =>
                f.RutaId == rutaId &&
                f.Activa &&
                (f.VigenteHasta == null || f.VigenteHasta >= hoy))
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();
    }

    public async Task<bool> ExistsByRutaClaseNombreAsync(
        int rutaId, int claseServicioId, string nombre)
        => await _dbSet
            .AnyAsync(f =>
                f.RutaId == rutaId &&
                f.ClaseServicioId == claseServicioId &&
                f.Nombre.ToLower() == nombre.ToLower());
}