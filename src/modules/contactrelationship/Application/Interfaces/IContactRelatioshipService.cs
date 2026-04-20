// src/modules/contactrelationship/Application/Interfaces/IContactRelationshipService.cs
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Application.Interfaces;

public interface IContactRelationshipService
{
    Task<ContactRelationshipEntity> CreateAsync(string nombre);
    Task<ContactRelationshipEntity?> GetByIdAsync(int id);
    Task<IEnumerable<ContactRelationshipEntity>> GetAllAsync();
    Task<ContactRelationshipEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}