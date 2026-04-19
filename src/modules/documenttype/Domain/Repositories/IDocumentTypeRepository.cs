// src/modules/documenttype/Domain/Repositories/IDocumentTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Domain.Repositories;

public interface IDocumentTypeRepository : IRepository<DocumentTypeEntity>
{
    Task<DocumentTypeEntity?> GetByDescripcionAsync(string descripcion);
    Task<bool> ExistsByDescripcionAsync(string descripcion);
}