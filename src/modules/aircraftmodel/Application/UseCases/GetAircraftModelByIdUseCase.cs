// src/modules/aircraftmodel/Application/UseCases/GetAircraftModelByIdUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class GetAircraftModelByIdUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAircraftModelByIdUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftModelEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del modelo no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {id}.");
    }
}