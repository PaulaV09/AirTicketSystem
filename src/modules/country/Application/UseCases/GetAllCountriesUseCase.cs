// src/modules/country/Application/UseCases/GetAllCountriesUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class GetAllCountriesUseCase
{
    private readonly ICountryRepository _repository;

    public GetAllCountriesUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CountryEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(c => c.Nombre);
}