// src/modules/luggagerestriction/Domain/Repositories/ILuggageRestrictionRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

public interface ILuggageRestrictionRepository : IRepository<LuggageRestrictionEntity>
{
    Task<IEnumerable<LuggageRestrictionEntity>> GetByTarifaAsync(int tarifaId);
    Task<LuggageRestrictionEntity?> GetByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId);
    Task<bool> ExistsByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId);
}