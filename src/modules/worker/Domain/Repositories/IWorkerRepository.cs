// src/modules/worker/Domain/Repositories/IWorkerRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Domain.Repositories;

public interface IWorkerRepository : IRepository<WorkerEntity>
{
    Task<WorkerEntity?> GetByPersonaAsync(int personaId);
    Task<WorkerEntity?> GetByUsuarioAsync(int usuarioId);
    Task<IEnumerable<WorkerEntity>> GetByAerolineaAsync(int aerolineaId);
    Task<IEnumerable<WorkerEntity>> GetByAeropuertoBaseAsync(int aeropuertoId);
    Task<IEnumerable<WorkerEntity>> GetByTipoTrabajadorAsync(int tipoTrabajadorId);
    Task<IEnumerable<WorkerEntity>> GetActivosAsync();
    Task<IEnumerable<WorkerEntity>> GetPilotosHabilitadosParaModeloAsync(
        int modeloAvionId);
}