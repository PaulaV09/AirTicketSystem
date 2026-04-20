// src/modules/country/Application/UseCases/CreateCountryUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.country.Domain.ValueObjects;
using AirTicketSystem.modules.continent.Domain.Repositories;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class CreateCountryUseCase
{
    private readonly ICountryRepository _repository;
    private readonly IContinentRepository _continentRepository;

    public CreateCountryUseCase(
        ICountryRepository repository,
        IContinentRepository continentRepository)
    {
        _repository          = repository;
        _continentRepository = continentRepository;
    }

    public async Task<CountryEntity> ExecuteAsync(
        int continenteId,
        string nombre,
        string codigoIso2,
        string codigoIso3)
    {
        // Validar que el continente exista
        var continente = await _continentRepository.GetByIdAsync(continenteId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {continenteId}.");

        var nombreVO = NombreCountry.Crear(nombre);
        var iso2VO   = CodigoIso2Country.Crear(codigoIso2);
        var iso3VO   = CodigoIso3Country.Crear(codigoIso3);

        if (await _repository.ExistsByCodigoIso2Async(iso2VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un país con el código ISO2 '{iso2VO.Valor}'.");

        if (await _repository.ExistsByCodigoIso3Async(iso3VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un país con el código ISO3 '{iso3VO.Valor}'.");

        var entity = new CountryEntity
        {
            ContinenteId = continenteId,
            Nombre       = nombreVO.Valor,
            CodigoIso2   = iso2VO.Valor,
            CodigoIso3   = iso3VO.Valor
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}