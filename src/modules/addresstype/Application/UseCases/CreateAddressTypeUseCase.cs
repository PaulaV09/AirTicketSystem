// src/modules/addresstype/Application/UseCases/CreateAddressTypeUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public class CreateAddressTypeUseCase
{
    private readonly IAddressTypeRepository _repository;

    public CreateAddressTypeUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressTypeEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = DescripcionAddressType.Crear(nombre);

        if (await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de dirección con el nombre '{nombreVO.Valor}'.");

        var entity = new AddressTypeEntity { Descripcion = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}