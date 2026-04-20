
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Application.Interfaces;

public interface IDocumentTypeService
{
    Task<DocumentTypeEntity> CreateAsync(string nombre);
    Task<DocumentTypeEntity?> GetByIdAsync(int id);
    Task<IEnumerable<DocumentTypeEntity>> GetAllAsync();
    Task<DocumentTypeEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}