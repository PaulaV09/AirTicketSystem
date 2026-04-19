// src/modules/phonetype/Domain/Repositories/IPhoneTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Domain.Repositories;

public interface IPhoneTypeRepository : IRepository<PhoneTypeEntity>
{
    Task<PhoneTypeEntity?> GetByDescripcionAsync(string descripcion);
    Task<bool> ExistsByDescripcionAsync(string descripcion);
}