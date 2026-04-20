// src/modules/pilotrating/Application/UseCases/GetRatingsByAircraftModelUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class GetRatingsByAircraftModelUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetRatingsByAircraftModelUseCase(IPilotRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PilotRatingEntity>> ExecuteAsync(int modeloAvionId)
    {
        if (modeloAvionId <= 0)
            throw new ArgumentException(
                "El ID del modelo de avión no es válido.");

        return await _repository.GetByModeloAvionAsync(modeloAvionId);
    }
}