// src/modules/country/Application/UseCases/UpdateCountryUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.country.Domain.ValueObjects;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class UpdateCountryUseCase
{
    private readonly ICountryRepository _repository;

    public UpdateCountryUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CountryEntity> ExecuteAsync(
        int id, string nombre, string codigoIso2, string codigoIso3)
    {
        var pais = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {id}.");

        var nombreVO = NombreCountry.Crear(nombre);
        var iso2VO   = CodigoIso2Country.Crear(codigoIso2);
        var iso3VO   = CodigoIso3Country.Crear(codigoIso3);

        if (iso2VO.Valor != pais.CodigoIso2 &&
            await _repository.ExistsByCodigoIso2Async(iso2VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro país con el código ISO2 '{iso2VO.Valor}'.");

        if (iso3VO.Valor != pais.CodigoIso3 &&
            await _repository.ExistsByCodigoIso3Async(iso3VO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro país con el código ISO3 '{iso3VO.Valor}'.");

        pais.Nombre     = nombreVO.Valor;
        pais.CodigoIso2 = iso2VO.Valor;
        pais.CodigoIso3 = iso3VO.Valor;

        await _repository.UpdateAsync(pais);
        return pais;
    }
}