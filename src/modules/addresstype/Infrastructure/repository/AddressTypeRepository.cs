// src/modules/addresstype/Infrastructure/repository/AddressTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Infrastructure.repository;

public class AddressTypeRepository
    : BaseRepository<AddressTypeEntity>, IAddressTypeRepository
{
    public AddressTypeRepository(AppDbContext context) : base(context) { }

    public async Task<AddressTypeEntity?> GetByDescripcionAsync(string descripcion)
        => await _dbSet
            .FirstOrDefaultAsync(a =>
                a.Descripcion.ToLower() == descripcion.ToLower());

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _dbSet
            .AnyAsync(a => a.Descripcion.ToLower() == descripcion.ToLower());
}