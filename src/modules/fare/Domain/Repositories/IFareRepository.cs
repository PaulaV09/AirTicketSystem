// src/modules/fare/Domain/Repositories/IFareRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.fare.Infrastructure.entity;

namespace AirTicketSystem.modules.fare.Domain.Repositories;

public interface IFareRepository : IRepository<FareEntity>
{
    Task<IEnumerable<FareEntity>> GetByRutaAsync(int rutaId);
    Task<IEnumerable<FareEntity>> GetByRutaAndClaseAsync(
        int rutaId, int claseServicioId);
    Task<IEnumerable<FareEntity>> GetActivasAsync();
    Task<IEnumerable<FareEntity>> GetActivasByRutaAsync(int rutaId);
    Task<bool> ExistsByRutaClaseNombreAsync(
        int rutaId, int claseServicioId, string nombre);
}