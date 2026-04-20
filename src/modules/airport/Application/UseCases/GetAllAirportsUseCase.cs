// src/modules/airport/Application/UseCases/GetAllAirportsUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAllAirportsUseCase
{
    private readonly IAirportRepository _repository;

    public GetAllAirportsUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AirportEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(a => a.Nombre);
}