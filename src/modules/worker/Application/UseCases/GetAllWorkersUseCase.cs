// src/modules/worker/Application/UseCases/GetAllWorkersUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetAllWorkersUseCase
{
    private readonly IWorkerRepository _repository;

    public GetAllWorkersUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync()
        => await _repository.GetAllAsync();
}