// src/modules/region/Infrastructure/repository/RegionRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Infrastructure.repository;

public class RegionRepository : BaseRepository<RegionEntity>, IRegionRepository
{
    public RegionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<RegionEntity>> GetByPaisAsync(int paisId)
        => await _dbSet
            .Where(r => r.PaisId == paisId)
            .OrderBy(r => r.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAndPaisAsync(string nombre, int paisId)
        => await _dbSet
            .AnyAsync(r =>
                r.Nombre.ToLower() == nombre.ToLower() &&
                r.PaisId == paisId);
}