// src/modules/route/Application/UseCases/GetRoutesByDestinationUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRoutesByDestinationUseCase
{
    private readonly IRouteRepository _repository;

    public GetRoutesByDestinationUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RouteEntity>> ExecuteAsync(int destinoId)
    {
        if (destinoId <= 0)
            throw new ArgumentException(
                "El ID del aeropuerto de destino no es válido.");

        return await _repository.GetByDestinoAsync(destinoId);
    }
}