// src/modules/serviceclass/Application/Interfaces/IServiceClassService.cs
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Application.Interfaces;

public interface IServiceClassService
{
    Task<ServiceClassEntity> CreateAsync(
        string nombre, string codigo, string? descripcion);
    Task<ServiceClassEntity?> GetByIdAsync(int id);
    Task<IEnumerable<ServiceClassEntity>> GetAllAsync();
    Task<ServiceClassEntity> UpdateAsync(
        int id, string nombre, string? descripcion);
    Task DeleteAsync(int id);
}