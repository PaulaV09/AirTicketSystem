// src/modules/airline/Application/UseCases/CreateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class CreateAirlineUseCase
{
    private readonly IAirlineRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateAirlineUseCase(
        IAirlineRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<AirlineEntity> ExecuteAsync(
        int paisId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? nombreComercial,
        string? sitioWeb)
    {
        _ = await _countryRepository.GetByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var iataVO   = CodigoIataAerolinea.Crear(codigoIata);
        var icaoVO   = CodigoIcaoAerolinea.Crear(codigoIcao);
        var nombreVO = NombreAerolinea.Crear(nombre);

        if (await _repository.ExistsByCodigoIataAsync(iataVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una aerolínea con el código IATA '{iataVO.Valor}'.");

        if (await _repository.ExistsByCodigoIcaoAsync(icaoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una aerolínea con el código ICAO '{icaoVO.Valor}'.");

        var entity = new AirlineEntity
        {
            PaisId          = paisId,
            CodigoIata      = iataVO.Valor,
            CodigoIcao      = icaoVO.Valor,
            Nombre          = nombreVO.Valor,
            NombreComercial = nombreComercial is not null
                ? NombreComercialAerolinea.Crear(nombreComercial).Valor
                : null,
            SitioWeb = sitioWeb is not null
                ? SitioWebAerolinea.Crear(sitioWeb).Valor
                : null,
            Activa = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}