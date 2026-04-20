// src/modules/pilotrating/Application/Interfaces/IPilotRatingService.cs
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.Interfaces;

public interface IPilotRatingService
{
    Task<PilotRatingEntity> CreateAsync(
        int licenciaId, int modeloAvionId,
        DateOnly fechaHabilitacion, DateOnly fechaVencimiento);
    Task<PilotRatingEntity?> GetByIdAsync(int id);
    Task<IEnumerable<PilotRatingEntity>> GetByLicenseAsync(int licenciaId);
    Task<IEnumerable<PilotRatingEntity>> GetByAircraftModelAsync(int modeloAvionId);
    Task<IEnumerable<PilotRatingEntity>> GetVigentesAsync();
    Task<PilotRatingEntity> RenewAsync(int id, DateOnly nuevaFechaVencimiento);
    Task DeleteAsync(int id);
}