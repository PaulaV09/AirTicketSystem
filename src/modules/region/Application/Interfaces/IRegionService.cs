// src/modules/region/Application/Interfaces/IRegionService.cs
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Application.Interfaces;

public interface IRegionService
{
    Task<RegionEntity> CreateAsync(int paisId, string nombre, string? codigo);
    Task<RegionEntity?> GetByIdAsync(int id);
    Task<IEnumerable<RegionEntity>> GetAllAsync();
    Task<IEnumerable<RegionEntity>> GetByCountryAsync(int paisId);
    Task<RegionEntity> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}