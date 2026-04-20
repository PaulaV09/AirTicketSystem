// src/modules/airport/Application/UseCases/GetAirportByIdUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAirportByIdUseCase
{
    private readonly IAirportRepository _repository;

    public GetAirportByIdUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<AirportEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del aeropuerto no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");
    }
}