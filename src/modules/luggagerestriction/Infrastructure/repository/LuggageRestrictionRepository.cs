// src/modules/luggagerestriction/Infrastructure/repository/LuggageRestrictionRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagerestriction.Infrastructure.repository;

public class LuggageRestrictionRepository
    : BaseRepository<LuggageRestrictionEntity>, ILuggageRestrictionRepository
{
    public LuggageRestrictionRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<LuggageRestrictionEntity>> GetByTarifaAsync(int tarifaId)
        => await _dbSet
            .Include(r => r.TipoEquipaje)
            .Where(r => r.TarifaId == tarifaId)
            .ToListAsync();

    public async Task<LuggageRestrictionEntity?> GetByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId)
        => await _dbSet
            .Include(r => r.TipoEquipaje)
            .FirstOrDefaultAsync(r =>
                r.TarifaId == tarifaId &&
                r.TipoEquipajeId == tipoEquipajeId);

    public async Task<bool> ExistsByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId)
        => await _dbSet
            .AnyAsync(r =>
                r.TarifaId == tarifaId &&
                r.TipoEquipajeId == tipoEquipajeId);
}