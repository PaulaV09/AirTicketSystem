// src/modules/country/Application/UseCases/GetCountriesByContinentUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class GetCountriesByContinentUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountriesByContinentUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CountryEntity>> ExecuteAsync(int continenteId)
    {
        if (continenteId <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        return await _repository.GetByContinenteAsync(continenteId);
    }
}