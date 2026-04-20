// src/modules/airline/Application/UseCases/DeactivateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class DeactivateAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public DeactivateAirlineUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var aerolinea = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");

        if (!aerolinea.Activa)
            throw new InvalidOperationException(
                $"La aerolínea '{aerolinea.Nombre}' ya se encuentra inactiva.");

        aerolinea.Activa = false;
        await _repository.UpdateAsync(aerolinea);
    }
}