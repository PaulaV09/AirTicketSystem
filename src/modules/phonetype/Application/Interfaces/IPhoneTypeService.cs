// src/modules/phonetype/Application/Interfaces/IPhoneTypeService.cs
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Application.Interfaces;

public interface IPhoneTypeService
{
    Task<PhoneTypeEntity> CreateAsync(string nombre);
    Task<PhoneTypeEntity?> GetByIdAsync(int id);
    Task<IEnumerable<PhoneTypeEntity>> GetAllAsync();
    Task<PhoneTypeEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}