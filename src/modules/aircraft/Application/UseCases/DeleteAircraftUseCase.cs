// src/modules/aircraft/Application/UseCases/DeleteAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class DeleteAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public DeleteAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        if (avion.Activo)
            throw new InvalidOperationException(
                $"No se puede eliminar el avión '{avion.Matricula}' " +
                "porque está activo. Debe darlo de baja primero.");

        if (avion.Estado == "EN_VUELO")
            throw new InvalidOperationException(
                $"No se puede eliminar el avión '{avion.Matricula}' " +
                "porque está en vuelo.");

        await _repository.DeleteAsync(id);
    }
}