// src/modules/worker/Domain/Repositories/IWorkerSpecialtyRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Domain.Repositories;

public interface IWorkerSpecialtyRepository : IRepository<WorkerSpecialtyEntity>
{
    Task<IEnumerable<WorkerSpecialtyEntity>> GetByTrabajadorAsync(int trabajadorId);
    Task<IEnumerable<WorkerSpecialtyEntity>> GetByEspecialidadAsync(int especialidadId);
    Task<bool> ExistsByTrabajadorAndEspecialidadAsync(
        int trabajadorId, int especialidadId);
}