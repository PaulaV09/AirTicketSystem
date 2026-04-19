// src/modules/person/Domain/Repositories/IPersonRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonRepository : IRepository<PersonEntity>
{
    Task<PersonEntity?> GetByDocumentoAsync(int tipoDocId, string numeroDoc);
    Task<PersonEntity?> GetByIdWithDetailsAsync(int id);
    Task<bool> ExistsByDocumentoAsync(int tipoDocId, string numeroDoc);
}