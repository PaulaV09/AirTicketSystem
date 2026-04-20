// src/modules/aircraftseat/Application/UseCases/GetAircraftSeatByIdUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class GetAircraftSeatByIdUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public GetAircraftSeatByIdUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftSeatEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del asiento no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");
    }
}