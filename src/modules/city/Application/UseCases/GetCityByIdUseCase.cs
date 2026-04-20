// src/modules/city/Application/UseCases/GetCityByIdUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class GetCityByIdUseCase
{
    private readonly ICityRepository _repository;

    public GetCityByIdUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<CityEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ciudad no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {id}.");
    }
}