// src/modules/airline/Infrastructure/repository/AirlineRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public class AirlineRepository : BaseRepository<AirlineEntity>, IAirlineRepository
{
    public AirlineRepository(AppDbContext context) : base(context) { }

    public async Task<AirlineEntity?> GetByCodigoIataAsync(string codigoIata)
        => await _dbSet
            .Include(a => a.Pais)
            .FirstOrDefaultAsync(a =>
                a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<AirlineEntity?> GetByCodigoIcaoAsync(string codigoIcao)
        => await _dbSet
            .Include(a => a.Pais)
            .FirstOrDefaultAsync(a =>
                a.CodigoIcao == codigoIcao.ToUpperInvariant());

    public async Task<IEnumerable<AirlineEntity>> GetActivasAsync()
        => await _dbSet
            .Include(a => a.Pais)
            .Where(a => a.Activa)
            .OrderBy(a => a.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoIataAsync(string codigoIata)
        => await _dbSet
            .AnyAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao)
        => await _dbSet
            .AnyAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());
}