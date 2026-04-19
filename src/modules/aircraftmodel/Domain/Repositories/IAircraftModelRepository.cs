// src/modules/aircraftmodel/Domain/Repositories/IAircraftModelRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Domain.Repositories;

public interface IAircraftModelRepository : IRepository<AircraftModelEntity>
{
    Task<AircraftModelEntity?> GetByCodigoModeloAsync(string codigoModelo);
    Task<IEnumerable<AircraftModelEntity>> GetByFabricanteAsync(int fabricanteId);
    Task<bool> ExistsByCodigoModeloAsync(string codigoModelo);
}