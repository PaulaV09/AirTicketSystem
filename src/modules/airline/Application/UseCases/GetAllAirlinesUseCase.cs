// src/modules/airline/Application/UseCases/GetAllAirlinesUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetAllAirlinesUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAllAirlinesUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AirlineEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(a => a.Nombre);
}