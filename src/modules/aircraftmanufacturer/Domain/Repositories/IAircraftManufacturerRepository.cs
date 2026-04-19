// src/modules/aircraftmanufacturer/Domain/Repositories/IAircraftManufacturerRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;

public interface IAircraftManufacturerRepository : IRepository<AircraftManufacturerEntity>
{
    Task<AircraftManufacturerEntity?> GetByNombreAsync(string nombre);
    Task<IEnumerable<AircraftManufacturerEntity>> GetByPaisAsync(int paisId);
    Task<bool> ExistsByNombreAsync(string nombre);
}