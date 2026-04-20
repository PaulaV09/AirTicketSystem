// src/modules/addresstype/Application/Interfaces/IAddressTypeService.cs
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Application.Interfaces;

public interface IAddressTypeService
{
    Task<AddressTypeEntity> CreateAsync(string nombre);
    Task<AddressTypeEntity?> GetByIdAsync(int id);
    Task<IEnumerable<AddressTypeEntity>> GetAllAsync();
    Task<AddressTypeEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}