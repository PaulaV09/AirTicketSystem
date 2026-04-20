// src/modules/city/Application/UseCases/UpdateCityUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.city.Domain.ValueObjects;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class UpdateCityUseCase
{
    private readonly ICityRepository _repository;

    public UpdateCityUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<CityEntity> ExecuteAsync(
        int id, string nombre, string? codigo)
    {
        var city = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {id}.");

        var nombreVO = NombreCity.Crear(nombre);

        if (nombreVO.Valor != city.Nombre &&
            await _repository.ExistsByNombreAndDepartamentoAsync(nombreVO.Valor, city.DepartamentoId))
            throw new InvalidOperationException(
                $"Ya existe otra ciudad con el nombre '{nombreVO.Valor}' " +
                $"en el mismo departamento.");

        city.Nombre = nombreVO.Valor;
        city.CodigoPostal = codigo is not null
            ? CodigoPostalCity.Crear(codigo).Valor
            : null;

        await _repository.UpdateAsync(city);
        return city;
    }
}