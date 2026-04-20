// src/modules/airport/Application/UseCases/GetActiveAirportsUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetActiveAirportsUseCase
{
    private readonly IAirportRepository _repository;

    public GetActiveAirportsUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AirportEntity>> ExecuteAsync()
        => await _repository.GetActivosAsync();
}