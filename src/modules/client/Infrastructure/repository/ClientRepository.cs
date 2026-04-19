// src/modules/client/Infrastructure/repository/ClientRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.repository;

public class ClientRepository : BaseRepository<ClientEntity>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context) { }

    public async Task<ClientEntity?> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.PersonaId == personaId);

    public async Task<ClientEntity?> GetByUsuarioAsync(int usuarioId)
        => await _dbSet
            .Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

    public async Task<ClientEntity?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(c => c.Persona)
                .ThenInclude(p => p.TipoDocumento)
            .Include(c => c.Persona)
                .ThenInclude(p => p.Genero)
            .Include(c => c.Persona)
                .ThenInclude(p => p.Telefonos)
            .Include(c => c.Persona)
                .ThenInclude(p => p.Emails)
            .Include(c => c.Persona)
                .ThenInclude(p => p.Direcciones)
            .Include(c => c.ContactosEmergencia)
                .ThenInclude(ce => ce.Persona)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<ClientEntity>> GetActivosAsync()
        => await _dbSet
            .Include(c => c.Persona)
            .Where(c => c.Activo)
            .OrderBy(c => c.Persona.Apellidos)
            .ToListAsync();
}