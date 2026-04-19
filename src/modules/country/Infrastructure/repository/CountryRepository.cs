// src/modules/country/Infrastructure/repository/CountryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Infrastructure.repository;

public class CountryRepository : BaseRepository<CountryEntity>, ICountryRepository
{
    public CountryRepository(AppDbContext context) : base(context) { }

    public async Task<CountryEntity?> GetByCodigoIso2Async(string codigoIso2)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.CodigoIso2 == codigoIso2.ToUpperInvariant());

    public async Task<CountryEntity?> GetByCodigoIso3Async(string codigoIso3)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.CodigoIso3 == codigoIso3.ToUpperInvariant());

    public async Task<IEnumerable<CountryEntity>> GetByContinenteAsync(int continenteId)
        => await _dbSet
            .Where(c => c.ContinenteId == continenteId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoIso2Async(string codigoIso2)
        => await _dbSet
            .AnyAsync(c => c.CodigoIso2 == codigoIso2.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIso3Async(string codigoIso3)
        => await _dbSet
            .AnyAsync(c => c.CodigoIso3 == codigoIso3.ToUpperInvariant());
}