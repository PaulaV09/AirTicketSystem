// src/modules/person/Infrastructure/repository/PersonAddressRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public class PersonAddressRepository
    : BaseRepository<PersonAddressEntity>, IPersonAddressRepository
{
    public PersonAddressRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PersonAddressEntity>> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(a => a.TipoDireccion)
            .Include(a => a.Ciudad)
            .Where(a => a.PersonaId == personaId)
            .OrderByDescending(a => a.EsPrincipal)
            .ToListAsync();

    public async Task<PersonAddressEntity?> GetPrincipalByPersonaAsync(int personaId)
        => await _dbSet
            .Include(a => a.TipoDireccion)
            .Include(a => a.Ciudad)
            .FirstOrDefaultAsync(a =>
                a.PersonaId == personaId && a.EsPrincipal);

    public async Task<IEnumerable<PersonAddressEntity>> GetByPersonaAndTipoAsync(
        int personaId, int tipoDireccionId)
        => await _dbSet
            .Include(a => a.TipoDireccion)
            .Include(a => a.Ciudad)
            .Where(a =>
                a.PersonaId == personaId &&
                a.TipoDireccionId == tipoDireccionId)
            .ToListAsync();

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var direcciones = await _dbSet
            .Where(a => a.PersonaId == personaId && a.EsPrincipal)
            .ToListAsync();

        foreach (var direccion in direcciones)
            direccion.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}