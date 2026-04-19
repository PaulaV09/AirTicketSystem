// src/modules/worker/Infrastructure/repository/WorkerSpecialtyRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.repository;

public class WorkerSpecialtyRepository
    : BaseRepository<WorkerSpecialtyEntity>, IWorkerSpecialtyRepository
{
    public WorkerSpecialtyRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<WorkerSpecialtyEntity>> GetByTrabajadorAsync(
        int trabajadorId)
        => await _dbSet
            .Include(ws => ws.Especialidad)
            .Where(ws => ws.TrabajadorId == trabajadorId)
            .ToListAsync();

    public async Task<IEnumerable<WorkerSpecialtyEntity>> GetByEspecialidadAsync(
        int especialidadId)
        => await _dbSet
            .Include(ws => ws.Trabajador)
                .ThenInclude(w => w.Persona)
            .Where(ws => ws.EspecialidadId == especialidadId)
            .ToListAsync();

    public async Task<bool> ExistsByTrabajadorAndEspecialidadAsync(
        int trabajadorId, int especialidadId)
        => await _dbSet
            .AnyAsync(ws =>
                ws.TrabajadorId == trabajadorId &&
                ws.EspecialidadId == especialidadId);
}