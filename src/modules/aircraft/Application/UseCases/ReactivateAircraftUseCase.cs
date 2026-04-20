// src/modules/aircraft/Application/UseCases/ReactivateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class ReactivateAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public ReactivateAircraftUseCase(IAircraftRepository repository)
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
                $"El avión '{avion.Matricula}' ya se encuentra activo.");

        avion.Estado = "DISPONIBLE";
        avion.Activo = true;

        await _repository.UpdateAsync(avion);
    }
}