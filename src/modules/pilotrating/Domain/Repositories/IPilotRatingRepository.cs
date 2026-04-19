// src/modules/pilotrating/Domain/Repositories/IPilotRatingRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Domain.Repositories;

public interface IPilotRatingRepository : IRepository<PilotRatingEntity>
{
    Task<IEnumerable<PilotRatingEntity>> GetByLicenciaAsync(int licenciaId);
    Task<IEnumerable<PilotRatingEntity>> GetByModeloAvionAsync(int modeloAvionId);
    Task<PilotRatingEntity?> GetByLicenciaAndModeloAsync(
        int licenciaId, int modeloAvionId);
    Task<bool> ExistsByLicenciaAndModeloAsync(int licenciaId, int modeloAvionId);
    Task<IEnumerable<PilotRatingEntity>> GetVigentesAsync();
}