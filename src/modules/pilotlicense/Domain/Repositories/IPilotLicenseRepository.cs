// src/modules/pilotlicense/Domain/Repositories/IPilotLicenseRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Domain.Repositories;

public interface IPilotLicenseRepository : IRepository<PilotLicenseEntity>
{
    Task<PilotLicenseEntity?> GetByNumeroLicenciaAsync(string numeroLicencia);
    Task<IEnumerable<PilotLicenseEntity>> GetByTrabajadorAsync(int trabajadorId);
    Task<IEnumerable<PilotLicenseEntity>> GetVigentesAsync();
    Task<IEnumerable<PilotLicenseEntity>> GetProximasAVencerAsync(int diasUmbral);
    Task<bool> ExistsByNumeroLicenciaAsync(string numeroLicencia);
}