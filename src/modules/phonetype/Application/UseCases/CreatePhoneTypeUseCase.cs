// src/modules/phonetype/Application/UseCases/CreatePhoneTypeUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;
using AirTicketSystem.modules.phonetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public class CreatePhoneTypeUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public CreatePhoneTypeUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneTypeEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = DescripcionPhoneType.Crear(nombre);

        if (await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de teléfono con el nombre '{nombreVO.Valor}'.");

        var entity = new PhoneTypeEntity { Descripcion = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}