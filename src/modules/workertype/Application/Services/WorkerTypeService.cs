// src/modules/workertype/Application/Services/WorkerTypeService.cs
using AirTicketSystem.modules.workertype.Application.Interfaces;
using AirTicketSystem.modules.workertype.Application.UseCases;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Application.Services;

public class WorkerTypeService : IWorkerTypeService
{
    private readonly CreateWorkerTypeUseCase _create;
    private readonly GetWorkerTypeByIdUseCase _getById;
    private readonly GetAllWorkerTypesUseCase _getAll;
    private readonly UpdateWorkerTypeUseCase _update;
    private readonly DeleteWorkerTypeUseCase _delete;

    public WorkerTypeService(
        CreateWorkerTypeUseCase create,
        GetWorkerTypeByIdUseCase getById,
        GetAllWorkerTypesUseCase getAll,
        UpdateWorkerTypeUseCase update,
        DeleteWorkerTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<WorkerTypeEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<WorkerTypeEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<WorkerTypeEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<WorkerTypeEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}