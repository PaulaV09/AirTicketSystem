// src/modules/serviceclass/Infrastructure/repository/ServiceClassRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Infrastructure.repository;

public class ServiceClassRepository
    : BaseRepository<ServiceClassEntity>, IServiceClassRepository
{
    public ServiceClassRepository(AppDbContext context) : base(context) { }

    public async Task<ServiceClassEntity?> GetByCodigoAsync(string codigo)
        => await _dbSet
            .FirstOrDefaultAsync(s =>
                s.Codigo == codigo.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoAsync(string codigo)
        => await _dbSet
            .AnyAsync(s => s.Codigo == codigo.ToUpperInvariant());
}