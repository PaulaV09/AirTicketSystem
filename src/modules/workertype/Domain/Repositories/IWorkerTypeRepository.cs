// src/modules/workertype/Domain/Repositories/IWorkerTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Domain.Repositories;

public interface IWorkerTypeRepository : IRepository<WorkerTypeEntity>
{
    Task<WorkerTypeEntity?> GetByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
}