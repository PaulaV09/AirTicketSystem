// src/modules/luggage/Domain/Repositories/ILuggageRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.luggage.Infrastructure.entity;

namespace AirTicketSystem.modules.luggage.Domain.Repositories;

public interface ILuggageRepository : IRepository<LuggageEntity>
{
    Task<IEnumerable<LuggageEntity>> GetByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IEnumerable<LuggageEntity>> GetByVueloAsync(int vueloId);
    Task<LuggageEntity?> GetByCodigoEquipajeAsync(string codigoEquipaje);
    Task<IEnumerable<LuggageEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<LuggageEntity>> GetConIncidenciasAsync();
    Task<bool> ExistsByCodigoEquipajeAsync(string codigoEquipaje);
}