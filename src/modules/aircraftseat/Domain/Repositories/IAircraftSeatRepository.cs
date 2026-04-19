// src/modules/aircraftseat/Domain/Repositories/IAircraftSeatRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Domain.Repositories;

public interface IAircraftSeatRepository : IRepository<AircraftSeatEntity>
{
    Task<AircraftSeatEntity?> GetByCodigoAndAvionAsync(
        string codigoAsiento, int avionId);
    Task<IEnumerable<AircraftSeatEntity>> GetByAvionAsync(int avionId);
    Task<IEnumerable<AircraftSeatEntity>> GetByAvionAndClaseAsync(
        int avionId, int claseServicioId);
    Task<int> ContarAsientosByAvionAsync(int avionId);
    Task<bool> ExistsByCodigoAndAvionAsync(string codigoAsiento, int avionId);
}