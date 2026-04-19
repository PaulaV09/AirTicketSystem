// src/modules/person/Infrastructure/repository/PersonPhoneRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public class PersonPhoneRepository
    : BaseRepository<PersonPhoneEntity>, IPersonPhoneRepository
{
    public PersonPhoneRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PersonPhoneEntity>> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(p => p.TipoTelefono)
            .Where(p => p.PersonaId == personaId)
            .OrderByDescending(p => p.EsPrincipal)
            .ToListAsync();

    public async Task<PersonPhoneEntity?> GetPrincipalByPersonaAsync(int personaId)
        => await _dbSet
            .Include(p => p.TipoTelefono)
            .FirstOrDefaultAsync(p =>
                p.PersonaId == personaId && p.EsPrincipal);

    public async Task<bool> ExistsByNumeroAndPersonaAsync(string numero, int personaId)
        => await _dbSet
            .AnyAsync(p =>
                p.Numero == numero &&
                p.PersonaId == personaId);

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var telefonos = await _dbSet
            .Where(p => p.PersonaId == personaId && p.EsPrincipal)
            .ToListAsync();

        foreach (var telefono in telefonos)
            telefono.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}