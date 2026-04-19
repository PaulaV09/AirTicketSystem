// src/modules/airline/Domain/Repositories/IAirlineEmailRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlineEmailRepository : IRepository<AirlineEmailEntity>
{
    Task<IEnumerable<AirlineEmailEntity>> GetByAerolineaAsync(int aerolineaId);
    Task<AirlineEmailEntity?> GetPrincipalByAerolineaAsync(int aerolineaId);
    Task<bool> ExistsByEmailAndAerolineaAsync(string email, int aerolineaId);
    Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId);
}