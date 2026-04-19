// src/modules/emailtype/Infrastructure/repository/EmailTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Infrastructure.repository;

public class EmailTypeRepository : BaseRepository<EmailTypeEntity>, IEmailTypeRepository
{
    public EmailTypeRepository(AppDbContext context) : base(context) { }

    public async Task<EmailTypeEntity?> GetByDescripcionAsync(string descripcion)
        => await _dbSet
            .FirstOrDefaultAsync(e =>
                e.Descripcion.ToLower() == descripcion.ToLower());

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _dbSet
            .AnyAsync(e => e.Descripcion.ToLower() == descripcion.ToLower());
}