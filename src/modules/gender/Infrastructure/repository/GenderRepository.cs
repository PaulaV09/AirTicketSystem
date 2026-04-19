// src/modules/gender/Infrastructure/repository/GenderRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Infrastructure.repository;

public class GenderRepository : BaseRepository<GenderEntity>, IGenderRepository
{
    public GenderRepository(AppDbContext context) : base(context) { }

    public async Task<GenderEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .FirstOrDefaultAsync(g => g.Nombre.ToLower() == nombre.ToLower());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(g => g.Nombre.ToLower() == nombre.ToLower());
}