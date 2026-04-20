// src/modules/aircraftseat/Application/UseCases/ActivateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class ActivateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public ActivateAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var asiento = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        if (asiento.Activo)
            throw new InvalidOperationException(
                $"El asiento '{asiento.CodigoAsiento}' ya se encuentra activo.");

        asiento.Activo = true;
        await _repository.UpdateAsync(asiento);
    }
}