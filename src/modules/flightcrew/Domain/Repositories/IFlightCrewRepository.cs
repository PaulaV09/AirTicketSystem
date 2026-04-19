// src/modules/flightcrew/Domain/Repositories/IFlightCrewRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;

namespace AirTicketSystem.modules.flightcrew.Domain.Repositories;

public interface IFlightCrewRepository : IRepository<FlightCrewEntity>
{
    Task<IEnumerable<FlightCrewEntity>> GetByVueloAsync(int vueloId);
    Task<IEnumerable<FlightCrewEntity>> GetByTrabajadorAsync(int trabajadorId);
    Task<FlightCrewEntity?> GetByVueloAndRolAsync(int vueloId, string rol);
    Task<bool> VueloTienePilotoAsync(int vueloId);
    Task<bool> VueloTieneCopiloAsync(int vueloId);
    Task<bool> ExistsByVueloAndTrabajadorAsync(int vueloId, int trabajadorId);
    Task<bool> ExistsByVueloAndRolAsync(int vueloId, string rol);
}