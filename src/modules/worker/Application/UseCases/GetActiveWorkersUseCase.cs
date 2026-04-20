// src/modules/worker/Application/UseCases/GetActiveWorkersUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetActiveWorkersUseCase
{
    private readonly IWorkerRepository _repository;

    public GetActiveWorkersUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync()
        => await _repository.GetActivosAsync();
}