// src/modules/pilotlicense/Application/Interfaces/IPilotLicenseService.cs
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Application.Interfaces;

public interface IPilotLicenseService
{
    Task<PilotLicenseEntity> CreateAsync(
        int trabajadorId, string numeroLicencia, string tipoLicencia,
        DateOnly fechaExpedicion, DateOnly fechaVencimiento,
        string autoridadEmisora);
    Task<PilotLicenseEntity?> GetByIdAsync(int id);
    Task<IEnumerable<PilotLicenseEntity>> GetByWorkerAsync(int trabajadorId);
    Task<IEnumerable<PilotLicenseEntity>> GetVigentesAsync();
    Task<IEnumerable<PilotLicenseEntity>> GetProximasAVencerAsync(int diasUmbral);
    Task<PilotLicenseEntity> RenewAsync(int id, DateOnly nuevaFechaVencimiento);
    Task SuspendAsync(int id);
    Task ReactivateAsync(int id);
    Task DeleteAsync(int id);
}