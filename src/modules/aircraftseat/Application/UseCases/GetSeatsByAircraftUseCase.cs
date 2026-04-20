// src/modules/aircraftseat/Application/UseCases/GetSeatsByAircraftUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class GetSeatsByAircraftUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public GetSeatsByAircraftUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftSeatEntity>> ExecuteAsync(int avionId)
    {
        if (avionId <= 0)
            throw new ArgumentException("El ID del avión no es válido.");

        return await _repository.GetByAvionAsync(avionId);
    }
}