// src/modules/airline/Application/UseCases/DeleteAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class DeleteAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public DeleteAirlineUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var aerolinea = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");

        if (aerolinea.Activa)
            throw new InvalidOperationException(
                $"No se puede eliminar la aerolínea '{aerolinea.Nombre}' " +
                "porque está activa. Desactívela primero.");

        await _repository.DeleteAsync(id);
    }
}