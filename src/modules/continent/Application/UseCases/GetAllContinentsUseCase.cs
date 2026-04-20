// src/modules/continent/Application/UseCases/GetAllContinentsUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public class GetAllContinentsUseCase
{
    private readonly IContinentRepository _repository;

    public GetAllContinentsUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContinentEntity>> ExecuteAsync()
    {
        var continentes = await _repository.GetAllAsync();

        if (!continentes.Any())
            return Enumerable.Empty<ContinentEntity>();

        return continentes.OrderBy(c => c.Nombre);
    }
}