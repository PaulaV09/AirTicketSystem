// src/modules/airport/Application/UseCases/DeleteAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class DeleteAirportUseCase
{
    private readonly IAirportRepository _repository;

    public DeleteAirportUseCase(IAirportRepository repository)
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
                $"No se puede eliminar el aeropuerto '{aeropuerto.Nombre}' " +
                "porque está activo. Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}