// src/modules/documenttype/Infrastructure/repository/DocumentTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Infrastructure.repository;

public class DocumentTypeRepository
    : BaseRepository<DocumentTypeEntity>, IDocumentTypeRepository
{
    public DocumentTypeRepository(AppDbContext context) : base(context) { }

    public async Task<DocumentTypeEntity?> GetByDescripcionAsync(string descripcion)
        => await _dbSet
            .FirstOrDefaultAsync(d =>
                d.Descripcion.ToLower() == descripcion.ToLower());

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _dbSet
            .AnyAsync(d => d.Descripcion.ToLower() == descripcion.ToLower());
}