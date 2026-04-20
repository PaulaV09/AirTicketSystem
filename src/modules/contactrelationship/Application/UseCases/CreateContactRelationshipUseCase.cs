// src/modules/contactrelationship/Application/UseCases/CreateContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;
using AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public class CreateContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public CreateContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationshipEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = DescripcionContactRelationship.Crear(nombre);

        if (await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de relación de contacto con el nombre '{nombreVO.Valor}'.");

        var entity = new ContactRelationshipEntity { Descripcion = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}