// src/modules/serviceclass/Domain/Repositories/IServiceClassRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Domain.Repositories;

public interface IServiceClassRepository : IRepository<ServiceClassEntity>
{
    Task<ServiceClassEntity?> GetByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoAsync(string codigo);
}