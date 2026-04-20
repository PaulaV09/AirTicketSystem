// src/modules/route/Application/Interfaces/IRouteService.cs
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Application.Interfaces;

public interface IRouteService
{
    Task<RouteEntity> CreateAsync(
        int aerolineaId, int origenId, int destinoId,
        int? distanciaKm, int? duracionMin);
    Task<RouteEntity?> GetByIdAsync(int id);
    Task<IEnumerable<RouteEntity>> GetAllAsync();
    Task<IEnumerable<RouteEntity>> GetByAirlineAsync(int aerolineaId);
    Task<IEnumerable<RouteEntity>> GetByOriginAsync(int origenId);
    Task<IEnumerable<RouteEntity>> GetByDestinationAsync(int destinoId);
    Task<IEnumerable<RouteEntity>> SearchAsync(int origenId, int destinoId);
    Task<IEnumerable<RouteEntity>> GetActivasAsync();
    Task<RouteEntity> UpdateAsync(
        int id, int? distanciaKm, int? duracionMin);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}