// src/modules/pilotrating/Application/UseCases/GetViigenteRatingsUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class GetVigenteRatingsUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetVigenteRatingsUseCase(IPilotRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PilotRatingEntity>> ExecuteAsync()
        => await _repository.GetVigentesAsync();
}