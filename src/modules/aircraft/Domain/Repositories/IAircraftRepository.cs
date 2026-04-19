// src/modules/aircraft/Domain/Repositories/IAircraftRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Domain.Repositories;

public interface IAircraftRepository : IRepository<AircraftEntity>
{
    Task<AircraftEntity?> GetByMatriculaAsync(string matricula);
    Task<IEnumerable<AircraftEntity>> GetByAerolineaAsync(int aerolineaId);
    Task<IEnumerable<AircraftEntity>> GetByModeloAsync(int modeloAvionId);
    Task<IEnumerable<AircraftEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<AircraftEntity>> GetDisponiblesAsync();
    Task<IEnumerable<AircraftEntity>> GetConMantenimientoUrgenteAsync();
    Task<bool> ExistsByMatriculaAsync(string matricula);
}