// src/modules/worker/Application/UseCases/GetWorkerByIdUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetWorkerByIdUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkerByIdUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");
    }
}