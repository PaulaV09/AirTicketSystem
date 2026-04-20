// src/modules/worker/Application/UseCases/DeleteWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class DeleteWorkerUseCase
{
    private readonly IWorkerRepository _repository;

    public DeleteWorkerUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var trabajador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (trabajador.Activo)
            throw new InvalidOperationException(
                "No se puede eliminar un trabajador activo. " +
                "Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}