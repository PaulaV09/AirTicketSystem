// src/modules/user/Domain/Repositories/IUserRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Domain.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<UserEntity?> GetByPersonaAsync(int personaId);
    Task<IEnumerable<UserEntity>> GetByRolAsync(int rolId);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<IEnumerable<UserEntity>> GetActivosAsync();
}