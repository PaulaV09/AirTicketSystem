// src/modules/route/Application/UseCases/GetAllRoutesUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetAllRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public GetAllRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RouteEntity>> ExecuteAsync()
        => await _repository.GetAllAsync();
}