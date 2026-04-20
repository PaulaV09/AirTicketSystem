// src/modules/aircraftmodel/Application/UseCases/GetAllAircraftModelsUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class GetAllAircraftModelsUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAllAircraftModelsUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftModelEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(m => m.Nombre);
}