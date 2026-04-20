// src/modules/route/Application/UseCases/GetRoutesByAirlineUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRoutesByAirlineUseCase
{
    private readonly IRouteRepository _repository;

    public GetRoutesByAirlineUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RouteEntity>> ExecuteAsync(int aerolineaId)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.GetByAerolineaAsync(aerolineaId);
    }
}