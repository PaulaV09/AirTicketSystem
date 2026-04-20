// src/modules/aircraft/Application/UseCases/GetAircraftWithUrgentMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftWithUrgentMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftWithUrgentMaintenanceUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftEntity>> ExecuteAsync()
        => await _repository.GetConMantenimientoUrgenteAsync();
}