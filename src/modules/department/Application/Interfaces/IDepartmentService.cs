// src/modules/department/Application/Interfaces/IDepartmentService.cs
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentEntity> CreateAsync(int paisId, string nombre, string? codigo);
    Task<DepartmentEntity?> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentEntity>> GetAllAsync();
    Task<IEnumerable<DepartmentEntity>> GetByCountryAsync(int paisId);
    Task<DepartmentEntity> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}