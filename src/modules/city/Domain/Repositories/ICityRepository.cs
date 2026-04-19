// src/modules/city/Domain/Repositories/ICityRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Domain.Repositories;

public interface ICityRepository : IRepository<CityEntity>
{
    Task<IEnumerable<CityEntity>> GetByDepartamentoAsync(int departamentoId);
    Task<bool> ExistsByNombreAndDepartamentoAsync(string nombre, int departamentoId);
}