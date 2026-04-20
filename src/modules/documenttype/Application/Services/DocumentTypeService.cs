// src/modules/documenttype/Application/Services/DocumentTypeService.cs
using AirTicketSystem.modules.documenttype.Application.Interfaces;
using AirTicketSystem.modules.documenttype.Application.UseCases;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Application.Services;

public class DocumentTypeService : IDocumentTypeService
{
    private readonly CreateDocumentTypeUseCase _create;
    private readonly GetDocumentTypeByIdUseCase _getById;
    private readonly GetAllDocumentTypesUseCase _getAll;
    private readonly UpdateDocumentTypeUseCase _update;
    private readonly DeleteDocumentTypeUseCase _delete;

    public DocumentTypeService(
        CreateDocumentTypeUseCase create,
        GetDocumentTypeByIdUseCase getById,
        GetAllDocumentTypesUseCase getAll,
        UpdateDocumentTypeUseCase update,
        DeleteDocumentTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<DocumentTypeEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<DocumentTypeEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<DocumentTypeEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<DocumentTypeEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}