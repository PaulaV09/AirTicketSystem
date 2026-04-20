// src/modules/gender/Application/Interfaces/IGenderService.cs
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Application.Interfaces;

public interface IGenderService
{
    Task<GenderEntity> CreateAsync(string nombre);
    Task<GenderEntity?> GetByIdAsync(int id);
    Task<IEnumerable<GenderEntity>> GetAllAsync();
    Task<GenderEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}