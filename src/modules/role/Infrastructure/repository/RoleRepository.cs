// src/modules/role/Infrastructure/repository/RoleRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.role.Infrastructure.entity;

namespace AirTicketSystem.modules.role.Infrastructure.repository;

public class RoleRepository : BaseRepository<RoleEntity>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context) { }

    public async Task<RoleEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .FirstOrDefaultAsync(r =>
                r.Nombre == nombre.ToUpperInvariant());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(r => r.Nombre == nombre.ToUpperInvariant());
}