// src/modules/aircraftseat/Application/UseCases/GetSeatsByAircraftAndClassUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class GetSeatsByAircraftAndClassUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public GetSeatsByAircraftAndClassUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftSeatEntity>> ExecuteAsync(
        int avionId, int claseServicioId)
    {
        if (avionId <= 0)
            throw new ArgumentException("El ID del avión no es válido.");

        if (claseServicioId <= 0)
            throw new ArgumentException(
                "El ID de la clase de servicio no es válido.");

        return await _repository.GetByAvionAndClaseAsync(avionId, claseServicioId);
    }
}