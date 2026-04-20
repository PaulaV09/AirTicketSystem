// src/modules/worker/Application/UseCases/RemoveWorkerSpecialtyUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class RemoveWorkerSpecialtyUseCase
{
    private readonly IWorkerSpecialtyRepository _repository;

    public RemoveWorkerSpecialtyUseCase(IWorkerSpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int workerSpecialtyId)
    {
        _ = await _repository.GetByIdAsync(workerSpecialtyId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la asignación de especialidad con ID " +
                $"{workerSpecialtyId}.");

        await _repository.DeleteAsync(workerSpecialtyId);
    }
}