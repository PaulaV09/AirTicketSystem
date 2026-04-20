// src/modules/contactrelationship/Application/UseCases/DeleteContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public class DeleteContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public DeleteContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de relación de contacto con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}