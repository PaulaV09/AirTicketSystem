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

    Task SaveAsync(MilesMovimiento movimiento);
}
