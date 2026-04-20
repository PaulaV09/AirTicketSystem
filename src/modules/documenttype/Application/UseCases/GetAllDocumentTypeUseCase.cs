// src/modules/documenttype/Application/UseCases/GetAllDocumentTypesUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public class GetAllDocumentTypesUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetAllDocumentTypesUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DocumentTypeEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(dt => dt.Descripcion);
}