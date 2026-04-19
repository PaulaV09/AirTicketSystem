// src/modules/addresstype/Domain/Repositories/IAddressTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Domain.Repositories;

public interface IAddressTypeRepository : IRepository<AddressTypeEntity>
{
    Task<AddressTypeEntity?> GetByDescripcionAsync(string descripcion);
    Task<bool> ExistsByDescripcionAsync(string descripcion);
}