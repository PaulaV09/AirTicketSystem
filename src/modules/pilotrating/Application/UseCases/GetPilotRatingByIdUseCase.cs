// src/modules/pilotrating/Application/UseCases/GetPilotRatingByIdUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class GetPilotRatingByIdUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetPilotRatingByIdUseCase(IPilotRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<PilotRatingEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la habilitación no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una habilitación con ID {id}.");
    }
}