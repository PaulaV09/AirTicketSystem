// src/modules/airport/Application/UseCases/CreateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.airport.Domain.ValueObjects;
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class CreateAirportUseCase
{
    private readonly IAirportRepository _repository;
    private readonly ICityRepository _cityRepository;

    public CreateAirportUseCase(
        IAirportRepository repository,
        ICityRepository cityRepository)
    {
        _repository     = repository;
        _cityRepository = cityRepository;
    }

    public async Task<AirportEntity> ExecuteAsync(
        int ciudadId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? direccion)
    {
        _ = await _cityRepository.GetByIdAsync(ciudadId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {ciudadId}.");

        var iataVO     = CodigoIataAirport.Crear(codigoIata);
        var icaoVO     = CodigoIcaoAirport.Crear(codigoIcao);
        var nombreVO   = NombreAirport.Crear(nombre);

        if (await _repository.ExistsByCodigoIataAsync(iataVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un aeropuerto con el código IATA '{iataVO.Valor}'.");

        if (await _repository.ExistsByCodigoIcaoAsync(icaoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un aeropuerto con el código ICAO '{icaoVO.Valor}'.");

        var entity = new AirportEntity
        {
            CiudadId    = ciudadId,
            CodigoIata  = iataVO.Valor,
            CodigoIcao  = icaoVO.Valor,
            Nombre      = nombreVO.Valor,
            Direccion   = direccion is not null
                ? DireccionAirport.Crear(direccion).Valor
                : null,
            Activo = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}