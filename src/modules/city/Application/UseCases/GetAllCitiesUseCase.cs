// src/modules/city/Application/UseCases/GetAllCitiesUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class GetAllCitiesUseCase
{
    private readonly ICityRepository _repository;

    public GetAllCitiesUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CityEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(c => c.Nombre);
}