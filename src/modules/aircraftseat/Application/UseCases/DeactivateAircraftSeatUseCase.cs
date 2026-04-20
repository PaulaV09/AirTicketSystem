// src/modules/aircraftseat/Application/UseCases/DeactivateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class DeactivateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public DeactivateAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var asiento = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        if (!asiento.Activo)
            throw new InvalidOperationException(
                $"El asiento '{asiento.CodigoAsiento}' ya se encuentra inactivo.");

        asiento.Activo = false;
        await _repository.UpdateAsync(asiento);
    }
}