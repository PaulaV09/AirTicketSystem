// src/modules/country/Application/UseCases/GetCountryByIdUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class GetCountryByIdUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountryByIdUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CountryEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {id}.");
    }
}