// src/modules/airport/Application/UseCases/GetAirportsByCityUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAirportsByCityUseCase
{
    private readonly IAirportRepository _repository;

    public GetAirportsByCityUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AirportEntity>> ExecuteAsync(int ciudadId)
    {
        if (ciudadId <= 0)
            throw new ArgumentException("El ID de la ciudad no es válido.");

        return await _repository.GetByCiudadAsync(ciudadId);
    }
}