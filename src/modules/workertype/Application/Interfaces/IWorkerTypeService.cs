// src/modules/workertype/Application/Interfaces/IWorkerTypeService.cs
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Application.Interfaces;

public interface IWorkerTypeService
{
    Task<WorkerTypeEntity> CreateAsync(string nombre);
    Task<WorkerTypeEntity?> GetByIdAsync(int id);
    Task<IEnumerable<WorkerTypeEntity>> GetAllAsync();
    Task<WorkerTypeEntity> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}