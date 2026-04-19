// src/modules/client/Domain/Repositories/IEmergencyContactRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Domain.Repositories;

public interface IEmergencyContactRepository : IRepository<EmergencyContactEntity>
{
    Task<IEnumerable<EmergencyContactEntity>> GetByClienteAsync(int clienteId);
    Task<EmergencyContactEntity?> GetPrincipalByClienteAsync(int clienteId);
    Task DesmarcarPrincipalByClienteAsync(int clienteId);
}