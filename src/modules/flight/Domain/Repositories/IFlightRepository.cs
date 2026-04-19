// src/modules/flight/Domain/Repositories/IFlightRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.flight.Domain.Repositories;

public interface IFlightRepository : IRepository<FlightEntity>
{
    Task<FlightEntity?> GetByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha);
    Task<IEnumerable<FlightEntity>> GetByRutaAsync(int rutaId);
    Task<IEnumerable<FlightEntity>> GetByAvionAsync(int avionId);
    Task<IEnumerable<FlightEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<FlightEntity>> GetByFechaAsync(DateTime fecha);
    Task<IEnumerable<FlightEntity>> GetByOrigenAndDestinoAsync(
        int origenId, int destinoId, DateTime fecha);
    Task<IEnumerable<FlightEntity>> GetProgramadosAsync();
    Task<IEnumerable<FlightEntity>> GetConCheckinAbiertoAsync();
    Task<bool> ExistsByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha);
}