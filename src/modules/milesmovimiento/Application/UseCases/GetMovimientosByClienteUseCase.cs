// src/modules/milesmovimiento/Application/UseCases/GetMovimientosByClienteUseCase.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milesmovimiento.Application.UseCases;

public sealed class GetMovimientosByClienteUseCase
{
    private readonly IMilesMovimientoRepository _movimientoRepo;
    private readonly IMilesCuentaRepository _cuentaRepo;

    public GetMovimientosByClienteUseCase(
        IMilesMovimientoRepository movimientoRepo,
        IMilesCuentaRepository cuentaRepo)
    {
        _movimientoRepo = movimientoRepo;
        _cuentaRepo     = cuentaRepo;
    }

    public async Task<IReadOnlyCollection<MilesMovimiento>> ExecuteAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var cuenta = await _cuentaRepo.FindByClienteAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"El cliente {clienteId} no tiene una cuenta de millas registrada.");

        return await _movimientoRepo.FindByCuentaAsync(cuenta.Id);
    }
}
