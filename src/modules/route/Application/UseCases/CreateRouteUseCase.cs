// src/modules/route/Application/UseCases/CreateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.route.Domain.ValueObjects;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class CreateRouteUseCase
{
    private readonly IRouteRepository _repository;
    private readonly IAirlineRepository _airlineRepository;
    private readonly IAirportRepository _airportRepository;

    public CreateRouteUseCase(
        IRouteRepository repository,
        IAirlineRepository airlineRepository,
        IAirportRepository airportRepository)
    {
        _repository        = repository;
        _airlineRepository = airlineRepository;
        _airportRepository = airportRepository;
    }

    public async Task<RouteEntity> ExecuteAsync(
        int aerolineaId,
        int origenId,
        int destinoId,
        int? distanciaKm,
        int? duracionMin)
    {
        var aerolinea = await _airlineRepository.GetByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        if (!aerolinea.Activa)
            throw new InvalidOperationException(
                $"La aerolínea '{aerolinea.Nombre}' está inactiva. " +
                "No se pueden crear rutas para aerolíneas inactivas.");

        var origen = await _airportRepository.GetByIdAsync(origenId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto de origen con ID {origenId}.");

        var destino = await _airportRepository.GetByIdAsync(destinoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto de destino con ID {destinoId}.");

        if (origenId == destinoId)
            throw new InvalidOperationException(
                "El aeropuerto de origen y destino no pueden ser el mismo.");

        if (!origen.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto de origen '{origen.Nombre}' está inactivo.");

        if (!destino.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto de destino '{destino.Nombre}' está inactivo.");

        if (await _repository.ExistsByAerolineaOrigenDestinoAsync(
            aerolineaId, origenId, destinoId))
            throw new InvalidOperationException(
                $"Ya existe una ruta de '{origen.Nombre}' a '{destino.Nombre}' " +
                $"para la aerolínea '{aerolinea.Nombre}'.");

        var entity = new RouteEntity
        {
            AerolineaId         = aerolineaId,
            OrigenId            = origenId,
            DestinoId           = destinoId,
            DistanciaKm         = distanciaKm is not null
                ? DistanciaKmRoute.Crear(distanciaKm.Value).Valor
                : null,
            DuracionEstimadaMin = duracionMin is not null
                ? DuracionEstimadaMinRoute.Crear(duracionMin.Value).Valor
                : null,
            Activa = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}