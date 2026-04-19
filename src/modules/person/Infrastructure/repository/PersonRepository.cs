// src/modules/person/Infrastructure/repository/PersonRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context) { }

    public async Task<PersonEntity?> GetByDocumentoAsync(int tipoDocId, string numeroDoc)
        => await _dbSet
            .Include(p => p.TipoDocumento)
            .Include(p => p.Genero)
            .Include(p => p.Nacionalidad)
            .FirstOrDefaultAsync(p =>
                p.TipoDocId == tipoDocId &&
                p.NumeroDoc == numeroDoc.ToUpperInvariant());

    public async Task<PersonEntity?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(p => p.TipoDocumento)
            .Include(p => p.Genero)
            .Include(p => p.Nacionalidad)
            .Include(p => p.Telefonos)
                .ThenInclude(t => t.TipoTelefono)
            .Include(p => p.Emails)
                .ThenInclude(e => e.TipoEmail)
            .Include(p => p.Direcciones)
                .ThenInclude(d => d.TipoDireccion)
            .Include(p => p.Direcciones)
                .ThenInclude(d => d.Ciudad)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> ExistsByDocumentoAsync(int tipoDocId, string numeroDoc)
        => await _dbSet
            .AnyAsync(p =>
                p.TipoDocId == tipoDocId &&
                p.NumeroDoc == numeroDoc.ToUpperInvariant());
}