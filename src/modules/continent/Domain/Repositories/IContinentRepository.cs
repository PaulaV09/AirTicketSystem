// src/modules/continent/Domain/Repositories/IContinentRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Domain.Repositories;

public interface IContinentRepository : IRepository<ContinentEntity>
{
    Task<ContinentEntity?> GetByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task<bool> ExistsByNombreAsync(string nombre);
}