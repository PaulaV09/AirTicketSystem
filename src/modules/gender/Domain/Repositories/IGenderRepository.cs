// src/modules/gender/Domain/Repositories/IGenderRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Domain.Repositories;

public interface IGenderRepository : IRepository<GenderEntity>
{
    Task<GenderEntity?> GetByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
}