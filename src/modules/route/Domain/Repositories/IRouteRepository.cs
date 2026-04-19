// src/modules/route/Domain/Repositories/IRouteRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Domain.Repositories;

public interface IRouteRepository : IRepository<RouteEntity>
{
    Task<IEnumerable<RouteEntity>> GetByAerolineaAsync(int aerolineaId);
    Task<IEnumerable<RouteEntity>> GetByOrigenAsync(int origenId);
    Task<IEnumerable<RouteEntity>> GetByDestinoAsync(int destinoId);
    Task<IEnumerable<RouteEntity>> GetByOrigenAndDestinoAsync(
        int origenId, int destinoId);
    Task<RouteEntity?> GetByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId);
    Task<IEnumerable<RouteEntity>> GetActivasAsync();
    Task<bool> ExistsByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId);
}