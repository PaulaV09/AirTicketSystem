// src/modules/aircraft/Application/UseCases/GetAircraftByAirlineUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftByAirlineUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByAirlineUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftEntity>> ExecuteAsync(int aerolineaId)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.GetByAerolineaAsync(aerolineaId);
    }
}