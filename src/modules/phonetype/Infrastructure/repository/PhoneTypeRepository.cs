// src/modules/phonetype/Infrastructure/repository/PhoneTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Infrastructure.repository;

public class PhoneTypeRepository : BaseRepository<PhoneTypeEntity>, IPhoneTypeRepository
{
    public PhoneTypeRepository(AppDbContext context) : base(context) { }

    public async Task<PhoneTypeEntity?> GetByDescripcionAsync(string descripcion)
        => await _dbSet
            .FirstOrDefaultAsync(p =>
                p.Descripcion.ToLower() == descripcion.ToLower());

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _dbSet
            .AnyAsync(p => p.Descripcion.ToLower() == descripcion.ToLower());
}