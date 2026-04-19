// src/modules/person/Domain/Repositories/IPersonEmailRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonEmailRepository : IRepository<PersonEmailEntity>
{
    Task<IEnumerable<PersonEmailEntity>> GetByPersonaAsync(int personaId);
    Task<PersonEmailEntity?> GetPrincipalByPersonaAsync(int personaId);
    Task<PersonEmailEntity?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}