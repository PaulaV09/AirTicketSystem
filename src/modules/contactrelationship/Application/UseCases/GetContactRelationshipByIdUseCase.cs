// src/modules/contactrelationship/Application/UseCases/GetContactRelationshipByIdUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public class GetContactRelationshipByIdUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public GetContactRelationshipByIdUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationshipEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de relación de contacto no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de relación de contacto con ID {id}.");
    }
}