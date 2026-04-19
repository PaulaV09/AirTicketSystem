// src/modules/contactrelationship/Infrastructure/repository/ContactRelationshipRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Infrastructure.repository;

public class ContactRelationshipRepository
    : BaseRepository<ContactRelationshipEntity>, IContactRelationshipRepository
{
    public ContactRelationshipRepository(AppDbContext context) : base(context) { }

    public async Task<ContactRelationshipEntity?> GetByDescripcionAsync(string descripcion)
        => await _dbSet
            .FirstOrDefaultAsync(c =>
                c.Descripcion.ToLower() == descripcion.ToLower());

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _dbSet
            .AnyAsync(c => c.Descripcion.ToLower() == descripcion.ToLower());
}