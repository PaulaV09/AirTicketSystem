// src/modules/contactrelationship/Application/Services/ContactRelationshipService.cs
using AirTicketSystem.modules.contactrelationship.Application.Interfaces;
using AirTicketSystem.modules.contactrelationship.Application.UseCases;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Application.Services;

public class ContactRelationshipService : IContactRelationshipService
{
    private readonly CreateContactRelationshipUseCase _create;
    private readonly GetContactRelationshipByIdUseCase _getById;
    private readonly GetAllContactRelationshipsUseCase _getAll;
    private readonly UpdateContactRelationshipUseCase _update;
    private readonly DeleteContactRelationshipUseCase _delete;

    public ContactRelationshipService(
        CreateContactRelationshipUseCase create,
        GetContactRelationshipByIdUseCase getById,
        GetAllContactRelationshipsUseCase getAll,
        UpdateContactRelationshipUseCase update,
        DeleteContactRelationshipUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<ContactRelationshipEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<ContactRelationshipEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<ContactRelationshipEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<ContactRelationshipEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}