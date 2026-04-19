// src/modules/role/Domain/Repositories/IRoleRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.role.Infrastructure.entity;

namespace AirTicketSystem.modules.role.Domain.Repositories;

public interface IRoleRepository : IRepository<RoleEntity>
{
    Task<RoleEntity?> GetByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
}