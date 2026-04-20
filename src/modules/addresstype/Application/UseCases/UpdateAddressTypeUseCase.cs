// src/modules/addresstype/Application/UseCases/UpdateAddressTypeUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public class UpdateAddressTypeUseCase
{
    private readonly IAddressTypeRepository _repository;

    public UpdateAddressTypeUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressTypeEntity> ExecuteAsync(int id, string nombre)
    {
        var tipoDireccion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de dirección con ID {id}.");

        var nombreVO = DescripcionAddressType.Crear(nombre);

        if (nombreVO.Valor != tipoDireccion.Descripcion &&
            await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de dirección con la descripción '{nombreVO.Valor}'.");

        tipoDireccion.Descripcion = nombreVO.Valor;
        await _repository.UpdateAsync(tipoDireccion);
        return tipoDireccion;
    }
}