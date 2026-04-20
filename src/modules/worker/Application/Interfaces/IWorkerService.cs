// src/modules/worker/Application/Interfaces/IWorkerService.cs
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.Interfaces;

public interface IWorkerService
{
    Task<WorkerEntity> CreateAsync(
        int personaId, int tipoTrabajadorId, int aeropuertoBaseId,
        DateOnly fechaContratacion, decimal salario,
        int? aerolineaId, int? usuarioId);
    Task<WorkerEntity?> GetByIdAsync(int id);
    Task<WorkerEntity?> GetByPersonAsync(int personaId);
    Task<IEnumerable<WorkerEntity>> GetAllAsync();
    Task<IEnumerable<WorkerEntity>> GetByAirlineAsync(int aerolineaId);
    Task<IEnumerable<WorkerEntity>> GetByAirportAsync(int aeropuertoId);
    Task<IEnumerable<WorkerEntity>> GetByWorkerTypeAsync(int tipoTrabajadorId);
    Task<IEnumerable<WorkerEntity>> GetActivosAsync();
    Task<IEnumerable<WorkerEntity>> GetPilotosHabilitadosAsync(int modeloAvionId);
    Task<WorkerEntity> UpdateSalaryAsync(int id, decimal nuevoSalario);
    Task<WorkerEntity> UpdateAirportAsync(int id, int aeropuertoBaseId);
    Task AssignSpecialtyAsync(int trabajadorId, int especialidadId);
    Task RemoveSpecialtyAsync(int trabajadorSpecialtyId);
    Task AssignUserAsync(int trabajadorId, int usuarioId);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}