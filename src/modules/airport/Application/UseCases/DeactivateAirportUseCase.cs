// src/modules/airport/Application/UseCases/DeactivateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class DeactivateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public DeactivateAirportUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var aeropuerto = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");

        if (!aeropuerto.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre}' ya se encuentra inactivo.");

        aeropuerto.Activo = false;
        await _repository.UpdateAsync(aeropuerto);
    }
}