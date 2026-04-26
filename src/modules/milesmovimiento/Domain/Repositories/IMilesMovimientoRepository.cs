// src/modules/milesmovimiento/Domain/Repositories/IMilesMovimientoRepository.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;

namespace AirTicketSystem.modules.milesmovimiento.Domain.Repositories;

public interface IMilesMovimientoRepository
{
    Task<MilesMovimiento?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<MilesMovimiento>> FindByCuentaAsync(int cuentaId);
    Task<IReadOnlyCollection<MilesMovimiento>> FindByReservaAsync(int reservaId);
    Task<IReadOnlyCollection<MilesMovimiento>> FindAllAsync();

    // Totales para reportes LINQ
    Task<int> SumarAcumulacionesByCuentaAsync(int cuentaId);
    Task<int> SumarRedencionesByCuentaAsync(int cuentaId);

    // Idempotencia: evita acumular millas dos veces en la misma reserva
    Task<bool> ExisteAcumulacionByReservaAsync(int reservaId);

    Task SaveAsync(MilesMovimiento movimiento);
}
