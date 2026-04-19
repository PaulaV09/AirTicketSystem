// src/modules/person/Domain/Repositories/IPersonPhoneRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonPhoneRepository : IRepository<PersonPhoneEntity>
{
    Task<IEnumerable<PersonPhoneEntity>> GetByPersonaAsync(int personaId);
    Task<PersonPhoneEntity?> GetPrincipalByPersonaAsync(int personaId);
    Task<bool> ExistsByNumeroAndPersonaAsync(string numero, int personaId);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}