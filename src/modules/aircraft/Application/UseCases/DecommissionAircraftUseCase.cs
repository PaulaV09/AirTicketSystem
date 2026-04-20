// src/modules/aircraft/Application/UseCases/DecommissionAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class DecommissionAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public DecommissionAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        if (avion.Estado == "EN_VUELO")
            throw new InvalidOperationException(
                $"No se puede dar de baja el avión '{avion.Matricula}' " +
                "porque está en vuelo.");

        if (!avion.Activo)
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' ya está dado de baja.");

        avion.Estado = "FUERA_DE_SERVICIO";
        avion.Activo = false;

        await _repository.UpdateAsync(avion);
    }
}