// src/modules/person/Domain/Repositories/IPersonAddressRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonAddressRepository : IRepository<PersonAddressEntity>
{
    Task<IEnumerable<PersonAddressEntity>> GetByPersonaAsync(int personaId);
    Task<PersonAddressEntity?> GetPrincipalByPersonaAsync(int personaId);
    Task<IEnumerable<PersonAddressEntity>> GetByPersonaAndTipoAsync(
        int personaId, int tipoDireccionId);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}