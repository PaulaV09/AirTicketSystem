// src/modules/route/Application/UseCases/GetActiveRoutesUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetActiveRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public GetActiveRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RouteEntity>> ExecuteAsync()
        => await _repository.GetActivasAsync();
}