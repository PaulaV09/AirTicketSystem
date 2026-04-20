// src/modules/phonetype/Application/UseCases/UpdatePhoneTypeUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;
using AirTicketSystem.modules.phonetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public class UpdatePhoneTypeUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public UpdatePhoneTypeUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneTypeEntity> ExecuteAsync(int id, string nombre)
    {
        var tipoTelefono = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {id}.");

        var nombreVO = DescripcionPhoneType.Crear(nombre);

        if (nombreVO.Valor != tipoTelefono.Descripcion &&
            await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de teléfono con la descripción '{nombreVO.Valor}'.");

        tipoTelefono.Descripcion = nombreVO.Valor;
        await _repository.UpdateAsync(tipoTelefono);
        return tipoTelefono;
    }
}