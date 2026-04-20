// src/modules/continent/Application/Interfaces/IContinentService.cs
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Application.Interfaces;

public interface IContinentService
{
    Task<ContinentEntity> CreateAsync(string nombre, string codigo);
    Task<ContinentEntity?> GetByIdAsync(int id);
    Task<IEnumerable<ContinentEntity>> GetAllAsync();
    Task<ContinentEntity> UpdateAsync(int id, string nombre, string codigo);
    Task DeleteAsync(int id);
}