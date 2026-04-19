// src/modules/specialty/Infrastructure/repository/SpecialtyRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Infrastructure.repository;

public class SpecialtyRepository : BaseRepository<SpecialtyEntity>, ISpecialtyRepository
{
    public SpecialtyRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<SpecialtyEntity>> GetByTipoTrabajadorAsync(
        int tipoTrabajadorId)
        => await _dbSet
            .Where(s => s.TipoTrabajadorId == tipoTrabajadorId)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<SpecialtyEntity>> GetGeneralesAsync()
        => await _dbSet
            .Where(s => s.TipoTrabajadorId == null)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(s => s.Nombre.ToLower() == nombre.ToLower());
}