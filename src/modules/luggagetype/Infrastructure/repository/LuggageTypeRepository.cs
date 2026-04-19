// src/modules/luggagetype/Infrastructure/repository/LuggageTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagetype.Infrastructure.repository;

public class LuggageTypeRepository
    : BaseRepository<LuggageTypeEntity>, ILuggageTypeRepository
{
    public LuggageTypeRepository(AppDbContext context) : base(context) { }

    public async Task<LuggageTypeEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .FirstOrDefaultAsync(l =>
                l.Nombre.ToLower() == nombre.ToLower());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(l => l.Nombre.ToLower() == nombre.ToLower());
}