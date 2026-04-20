// src/modules/documenttype/Application/UseCases/GetDocumentTypeByIdUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public class GetDocumentTypeByIdUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetDocumentTypeByIdUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentTypeEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de documento no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {id}.");
    }
}