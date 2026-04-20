// src/modules/aircraft/Application/UseCases/GetAllAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAllAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAllAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(a => a.Matricula);
}