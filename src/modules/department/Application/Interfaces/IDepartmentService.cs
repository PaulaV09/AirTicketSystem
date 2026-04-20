// src/modules/department/Application/Interfaces/IDepartmentService.cs
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentEntity> CreateAsync(int regionId, string nombre, string? codigo);
    Task<DepartmentEntity?> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentEntity>> GetAllAsync();
    Task<IEnumerable<DepartmentEntity>> GetByRegionAsync(int regionId);
    Task<DepartmentEntity> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}