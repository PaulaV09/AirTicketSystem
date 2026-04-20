// src/modules/gender/Application/UseCases/UpdateGenderUseCase.cs
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;
using AirTicketSystem.modules.gender.Domain.ValueObjects;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public class UpdateGenderUseCase
{
    private readonly IGenderRepository _repository;

    public UpdateGenderUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<GenderEntity> ExecuteAsync(int id, string nombre)
    {
        var genero = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un género con ID {id}.");

        var nombreVO = NombreGender.Crear(nombre);

        if (nombreVO.Valor != genero.Nombre &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro género con el nombre '{nombreVO.Valor}'.");

        genero.Nombre = nombreVO.Valor;
        await _repository.UpdateAsync(genero);
        return genero;
    }
}