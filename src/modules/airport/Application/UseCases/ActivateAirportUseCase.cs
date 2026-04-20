// src/modules/airport/Application/UseCases/ActivateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class ActivateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public ActivateAirportUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var aeropuerto = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");

        if (aeropuerto.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre}' ya se encuentra activo.");

        aeropuerto.Activo = true;
        await _repository.UpdateAsync(aeropuerto);
    }
}