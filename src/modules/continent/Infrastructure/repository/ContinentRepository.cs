// src/modules/continent/Infrastructure/repository/ContinentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Infrastructure.repository;

public class ContinentRepository : BaseRepository<ContinentEntity>, IContinentRepository
{
    public ContinentRepository(AppDbContext context) : base(context) { }

    public async Task<ContinentEntity?> GetByCodigoAsync(string codigo)
        => await _dbSet
            .FirstOrDefaultAsync(c => c.Codigo == codigo.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoAsync(string codigo)
        => await _dbSet
            .AnyAsync(c => c.Codigo == codigo.ToUpperInvariant());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(c => c.Nombre.ToLower() == nombre.ToLower());
}