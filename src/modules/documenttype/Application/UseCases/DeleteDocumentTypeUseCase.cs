// src/modules/documenttype/Application/UseCases/DeleteDocumentTypeUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.Repositories;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public class DeleteDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public DeleteDocumentTypeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}