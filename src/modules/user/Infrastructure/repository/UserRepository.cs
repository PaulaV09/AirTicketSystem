// src/modules/user/Infrastructure/repository/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Infrastructure.repository;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
        => await _dbSet
            .Include(u => u.Rol)
            .Include(u => u.Persona)
            .FirstOrDefaultAsync(u =>
                u.Username == username.ToLowerInvariant());

    public async Task<UserEntity?> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.PersonaId == personaId);

    public async Task<IEnumerable<UserEntity>> GetByRolAsync(int rolId)
        => await _dbSet
            .Include(u => u.Persona)
            .Where(u => u.RolId == rolId)
            .OrderBy(u => u.Username)
            .ToListAsync();

    public async Task<bool> ExistsByUsernameAsync(string username)
        => await _dbSet
            .AnyAsync(u => u.Username == username.ToLowerInvariant());

    public async Task<IEnumerable<UserEntity>> GetActivosAsync()
        => await _dbSet
            .Include(u => u.Rol)
            .Include(u => u.Persona)
            .Where(u => u.Activo)
            .OrderBy(u => u.Username)
            .ToListAsync();
}