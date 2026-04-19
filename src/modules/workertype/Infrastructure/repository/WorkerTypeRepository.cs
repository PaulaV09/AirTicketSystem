// src/modules/workertype/Infrastructure/repository/WorkerTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Infrastructure.repository;

public class WorkerTypeRepository
    : BaseRepository<WorkerTypeEntity>, IWorkerTypeRepository
{
    public WorkerTypeRepository(AppDbContext context) : base(context) { }

    public async Task<WorkerTypeEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .FirstOrDefaultAsync(w =>
                w.Nombre.ToLower() == nombre.ToLower());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(w => w.Nombre.ToLower() == nombre.ToLower());
}