// src/modules/emailtype/Domain/Repositories/IEmailTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Domain.Repositories;

public interface IEmailTypeRepository : IRepository<EmailTypeEntity>
{
    Task<EmailTypeEntity?> GetByDescripcionAsync(string descripcion);
    Task<bool> ExistsByDescripcionAsync(string descripcion);
}