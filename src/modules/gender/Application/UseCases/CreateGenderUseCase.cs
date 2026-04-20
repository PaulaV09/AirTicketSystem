// src/modules/gender/Application/UseCases/CreateGenderUseCase.cs
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;
using AirTicketSystem.modules.gender.Domain.ValueObjects;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public class CreateGenderUseCase
{
    private readonly IGenderRepository _repository;

    public CreateGenderUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<GenderEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = NombreGender.Crear(nombre);

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un género con el nombre '{nombreVO.Valor}'.");

        var entity = new GenderEntity { Nombre = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}