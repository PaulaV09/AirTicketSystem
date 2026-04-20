// src/modules/city/Application/Interfaces/ICityService.cs
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Application.Interfaces;

public interface ICityService
{
    Task<CityEntity> CreateAsync(int departmentId, string nombre, string? codigo);
    Task<CityEntity?> GetByIdAsync(int id);
    Task<IEnumerable<CityEntity>> GetAllAsync();
    Task<IEnumerable<CityEntity>> GetByDepartmentAsync(int departmentId);
    Task<CityEntity> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}