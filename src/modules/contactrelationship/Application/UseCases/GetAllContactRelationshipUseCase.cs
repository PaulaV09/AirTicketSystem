// src/modules/contactrelationship/Application/UseCases/GetAllContactRelationshipsUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public class GetAllContactRelationshipsUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public GetAllContactRelationshipsUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactRelationshipEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(et => et.Descripcion);
}