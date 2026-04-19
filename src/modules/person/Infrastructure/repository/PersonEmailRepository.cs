// src/modules/person/Infrastructure/repository/PersonEmailRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public class PersonEmailRepository
    : BaseRepository<PersonEmailEntity>, IPersonEmailRepository
{
    public PersonEmailRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PersonEmailEntity>> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(e => e.TipoEmail)
            .Where(e => e.PersonaId == personaId)
            .OrderByDescending(e => e.EsPrincipal)
            .ToListAsync();

    public async Task<PersonEmailEntity?> GetPrincipalByPersonaAsync(int personaId)
        => await _dbSet
            .Include(e => e.TipoEmail)
            .FirstOrDefaultAsync(e =>
                e.PersonaId == personaId && e.EsPrincipal);

    public async Task<PersonEmailEntity?> GetByEmailAsync(string email)
        => await _dbSet
            .Include(e => e.TipoEmail)
            .FirstOrDefaultAsync(e =>
                e.Email.ToLower() == email.ToLower());

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _dbSet
            .AnyAsync(e => e.Email.ToLower() == email.ToLower());

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var emails = await _dbSet
            .Where(e => e.PersonaId == personaId && e.EsPrincipal)
            .ToListAsync();

        foreach (var email in emails)
            email.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}