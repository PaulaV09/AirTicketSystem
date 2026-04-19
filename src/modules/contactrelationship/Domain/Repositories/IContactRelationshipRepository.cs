// src/modules/contactrelationship/Domain/Repositories/IContactRelationshipRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Domain.Repositories;

public interface IContactRelationshipRepository : IRepository<ContactRelationshipEntity>
{
    Task<ContactRelationshipEntity?> GetByDescripcionAsync(string descripcion);
    Task<bool> ExistsByDescripcionAsync(string descripcion);
}