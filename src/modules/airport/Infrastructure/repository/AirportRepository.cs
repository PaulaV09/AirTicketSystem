// src/modules/airport/Infrastructure/repository/AirportRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Infrastructure.repository;

public class AirportRepository : BaseRepository<AirportEntity>, IAirportRepository
{
    public AirportRepository(AppDbContext context) : base(context) { }

    public async Task<AirportEntity?> GetByCodigoIataAsync(string codigoIata)
        => await _dbSet
            .Include(a => a.Ciudad)
            .FirstOrDefaultAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<AirportEntity?> GetByCodigoIcaoAsync(string codigoIcao)
        => await _dbSet
            .Include(a => a.Ciudad)
            .FirstOrDefaultAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());

    public async Task<IEnumerable<AirportEntity>> GetByCiudadAsync(int ciudadId)
        => await _dbSet
            .Where(a => a.CiudadId == ciudadId)
            .OrderBy(a => a.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<AirportEntity>> GetActivosAsync()
        => await _dbSet
            .Include(a => a.Ciudad)
            .Where(a => a.Activo)
            .OrderBy(a => a.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoIataAsync(string codigoIata)
        => await _dbSet
            .AnyAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao)
        => await _dbSet
            .AnyAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());
}