// src/modules/client/Domain/Repositories/IClientRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Domain.Repositories;

public interface IClientRepository : IRepository<ClientEntity>
{
    Task<ClientEntity?> GetByPersonaAsync(int personaId);
    Task<ClientEntity?> GetByUsuarioAsync(int usuarioId);
    Task<ClientEntity?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<ClientEntity>> GetActivosAsync();
}