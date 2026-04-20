// src/modules/worker/Application/UseCases/DeactivateWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class DeactivateWorkerUseCase
{
    private readonly IWorkerRepository _repository;

    public DeactivateWorkerUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var trabajador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (!trabajador.Activo)
            throw new InvalidOperationException(
                "El trabajador ya se encuentra inactivo.");

        trabajador.Activo = false;
        await _repository.UpdateAsync(trabajador);
    }
}