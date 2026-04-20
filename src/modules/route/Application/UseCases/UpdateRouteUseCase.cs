// src/modules/route/Application/UseCases/UpdateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.route.Domain.ValueObjects;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class UpdateRouteUseCase
{
    private readonly IRouteRepository _repository;

    public UpdateRouteUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<RouteEntity> ExecuteAsync(
        int id, int? distanciaKm, int? duracionMin)
    {
        var ruta = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {id}.");

        ruta.DistanciaKm = distanciaKm is not null
            ? DistanciaKmRoute.Crear(distanciaKm.Value).Valor
            : null;

        ruta.DuracionEstimadaMin = duracionMin is not null
            ? DuracionEstimadaMinRoute.Crear(duracionMin.Value).Valor
            : null;

        await _repository.UpdateAsync(ruta);
        return ruta;
    }
}