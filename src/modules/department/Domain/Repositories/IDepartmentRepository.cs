// src/modules/department/Domain/Repositories/IDepartmentRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Domain.Repositories;

public interface IDepartmentRepository : IRepository<DepartmentEntity>
{
    Task<IEnumerable<DepartmentEntity>> GetByRegionAsync(int regionId);
    Task<bool> ExistsByNombreAndRegionAsync(string nombre, int regionId);
}