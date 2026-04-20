// src/modules/contactrelationship/Application/UseCases/UpdateContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;
using AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public class UpdateContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public UpdateContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationshipEntity> ExecuteAsync(int id, string nombre)
    {
        var tipoRelacionContacto = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de relación de contacto con ID {id}.");

        var nombreVO = DescripcionContactRelationship.Crear(nombre);

        if (nombreVO.Valor != tipoRelacionContacto.Descripcion &&
            await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de relación de contacto con la descripción '{nombreVO.Valor}'.");

        tipoRelacionContacto.Descripcion = nombreVO.Valor;
        await _repository.UpdateAsync(tipoRelacionContacto);
        return tipoRelacionContacto;
    }
}