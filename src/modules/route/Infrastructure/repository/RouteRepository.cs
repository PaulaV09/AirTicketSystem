// src/modules/route/Infrastructure/repository/RouteRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Infrastructure.repository;

public class RouteRepository : BaseRepository<RouteEntity>, IRouteRepository
{
    public RouteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<RouteEntity>> GetByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .Where(r => r.AerolineaId == aerolineaId)
            .OrderBy(r => r.Origen.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<RouteEntity>> GetByOrigenAsync(int origenId)
        => await _dbSet
            .Include(r => r.Aerolinea)
            .Include(r => r.Destino)
            .Where(r => r.OrigenId == origenId && r.Activa)
            .OrderBy(r => r.Destino.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<RouteEntity>> GetByDestinoAsync(int destinoId)
        => await _dbSet
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Where(r => r.DestinoId == destinoId && r.Activa)
            .OrderBy(r => r.Origen.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<RouteEntity>> GetByOrigenAndDestinoAsync(
        int origenId, int destinoId)
        => await _dbSet
            .Include(r => r.Aerolinea)
            .Where(r =>
                r.OrigenId == origenId &&
                r.DestinoId == destinoId &&
                r.Activa)
            .ToListAsync();

    public async Task<RouteEntity?> GetByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId)
        => await _dbSet
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .FirstOrDefaultAsync(r =>
                r.AerolineaId == aerolineaId &&
                r.OrigenId == origenId &&
                r.DestinoId == destinoId);

    public async Task<IEnumerable<RouteEntity>> GetActivasAsync()
        => await _dbSet
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .Where(r => r.Activa)
            .OrderBy(r => r.Aerolinea.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId)
        => await _dbSet
            .AnyAsync(r =>
                r.AerolineaId == aerolineaId &&
                r.OrigenId == origenId &&
                r.DestinoId == destinoId);
}