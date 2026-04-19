// src/modules/region/Domain/Repositories/IRegionRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Domain.Repositories;

public interface IRegionRepository : IRepository<RegionEntity>
{
    Task<IEnumerable<RegionEntity>> GetByPaisAsync(int paisId);
    Task<bool> ExistsByNombreAndPaisAsync(string nombre, int paisId);
}