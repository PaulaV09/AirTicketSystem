// src/modules/milesmovimiento/Application/Services/MilesMovimientoService.cs
using AirTicketSystem.modules.milesmovimiento.Application.Interfaces;
using AirTicketSystem.modules.milesmovimiento.Application.UseCases;
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;

namespace AirTicketSystem.modules.milesmovimiento.Application.Services;

public sealed class MilesMovimientoService : IMilesMovimientoService
{
    private readonly RegistrarAcumulacionUseCase _acumulacion;
    private readonly RegistrarRedencionUseCase _redencion;
    private readonly GetMovimientosByClienteUseCase _getByCliente;
    private readonly GetAllMovimientosUseCase _getAll;

    public MilesMovimientoService(
        RegistrarAcumulacionUseCase acumulacion,
        RegistrarRedencionUseCase redencion,
        GetMovimientosByClienteUseCase getByCliente,
        GetAllMovimientosUseCase getAll)
    {
        _acumulacion  = acumulacion;
        _redencion    = redencion;
        _getByCliente = getByCliente;
        _getAll       = getAll;
    }

    public Task<MilesMovimiento> RegistrarAcumulacionAsync(
        int clienteId, int reservaId, decimal valorReserva, string numeroVuelo)
        => _acumulacion.ExecuteAsync(clienteId, reservaId, valorReserva, numeroVuelo);

    public Task<MilesMovimiento> RegistrarRedencionAsync(
        int clienteId, int reservaId, int millasARedimir)
        => _redencion.ExecuteAsync(clienteId, reservaId, millasARedimir);

    public Task<IReadOnlyCollection<MilesMovimiento>> GetByClienteAsync(int clienteId)
        => _getByCliente.ExecuteAsync(clienteId);

    public Task<IReadOnlyCollection<MilesMovimiento>> GetAllAsync()
        => _getAll.ExecuteAsync();
}
