// src/modules/workertype/Application/UseCases/GetAllWorkerTypesUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class GetAllWorkerTypesUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public GetAllWorkerTypesUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerTypeEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(w => w.Nombre);
}