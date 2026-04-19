// src/modules/city/Infrastructure/repository/CityRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Infrastructure.repository;

public class CityRepository : BaseRepository<CityEntity>, ICityRepository
{
    public CityRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<CityEntity>> GetByDepartamentoAsync(int departamentoId)
        => await _dbSet
            .Where(c => c.DepartamentoId == departamentoId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAndDepartamentoAsync(
        string nombre, int departamentoId)
        => await _dbSet
            .AnyAsync(c =>
                c.Nombre.ToLower() == nombre.ToLower() &&
                c.DepartamentoId == departamentoId);
}