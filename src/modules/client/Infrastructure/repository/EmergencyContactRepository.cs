// src/modules/client/Infrastructure/repository/EmergencyContactRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.repository;

public class EmergencyContactRepository
    : BaseRepository<EmergencyContactEntity>, IEmergencyContactRepository
{
    public EmergencyContactRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<EmergencyContactEntity>> GetByClienteAsync(int clienteId)
        => await _dbSet
            .Include(ce => ce.Persona)
                .ThenInclude(p => p.Telefonos)
            .Include(ce => ce.Relacion)
            .Where(ce => ce.ClienteId == clienteId)
            .OrderByDescending(ce => ce.EsPrincipal)
            .ToListAsync();

    public async Task<EmergencyContactEntity?> GetPrincipalByClienteAsync(int clienteId)
        => await _dbSet
            .Include(ce => ce.Persona)
            .Include(ce => ce.Relacion)
            .FirstOrDefaultAsync(ce =>
                ce.ClienteId == clienteId && ce.EsPrincipal);

    public async Task DesmarcarPrincipalByClienteAsync(int clienteId)
    {
        var contactos = await _dbSet
            .Where(ce => ce.ClienteId == clienteId && ce.EsPrincipal)
            .ToListAsync();

        foreach (var contacto in contactos)
            contacto.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }
}