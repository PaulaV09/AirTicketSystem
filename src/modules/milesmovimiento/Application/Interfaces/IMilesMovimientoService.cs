// src/modules/milesmovimiento/Application/Interfaces/IMilesMovimientoService.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;

namespace AirTicketSystem.modules.milesmovimiento.Application.Interfaces;

public interface IMilesMovimientoService
{
    Task<MilesMovimiento> RegistrarAcumulacionAsync(
        int clienteId, int reservaId, decimal valorReserva, string numeroVuelo);

    Task<MilesMovimiento> RegistrarRedencionAsync(
        int clienteId, int reservaId, int millasARedimir);

    Task<IReadOnlyCollection<MilesMovimiento>> GetByClienteAsync(int clienteId);
    Task<IReadOnlyCollection<MilesMovimiento>> GetAllAsync();
}
