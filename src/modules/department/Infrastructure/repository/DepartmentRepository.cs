// src/modules/department/Infrastructure/repository/DepartmentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Infrastructure.repository;

public class DepartmentRepository : BaseRepository<DepartmentEntity>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<DepartmentEntity>> GetByRegionAsync(int regionId)
        => await _dbSet
            .Where(d => d.RegionId == regionId)
            .OrderBy(d => d.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAndRegionAsync(string nombre, int regionId)
        => await _dbSet
            .AnyAsync(d =>
                d.Nombre.ToLower() == nombre.ToLower() &&
                d.RegionId == regionId);
}