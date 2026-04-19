// src/modules/airline/Domain/Repositories/IAirlinePhoneRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlinePhoneRepository : IRepository<AirlinePhoneEntity>
{
    Task<IEnumerable<AirlinePhoneEntity>> GetByAerolineaAsync(int aerolineaId);
    Task<AirlinePhoneEntity?> GetPrincipalByAerolineaAsync(int aerolineaId);
    Task<bool> ExistsByNumeroAndAerolineaAsync(string numero, int aerolineaId);
    Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId);
}