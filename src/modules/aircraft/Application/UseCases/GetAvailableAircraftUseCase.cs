// src/modules/aircraft/Application/UseCases/GetAvailableAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAvailableAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAvailableAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftEntity>> ExecuteAsync()
        => await _repository.GetDisponiblesAsync();
}