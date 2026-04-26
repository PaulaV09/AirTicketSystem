// src/modules/milesmovimiento/Application/UseCases/RegistrarRedencionUseCase.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milesmovimiento.Application.UseCases;

public sealed class RegistrarRedencionUseCase
{
    private readonly IMilesMovimientoRepository _movimientoRepo;
    private readonly IMilesCuentaRepository _cuentaRepo;

    public RegistrarRedencionUseCase(
        IMilesMovimientoRepository movimientoRepo,
        IMilesCuentaRepository cuentaRepo)
    {
        _movimientoRepo = movimientoRepo;
        _cuentaRepo     = cuentaRepo;
    }

    public async Task<MilesMovimiento> ExecuteAsync(
        int clienteId,
        int reservaId,
        int millasARedimir,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        if (millasARedimir <= 0)
            throw new ArgumentException("La cantidad de millas a redimir debe ser mayor a 0.");

        // 1. Obtener la cuenta del cliente
        var cuenta = await _cuentaRepo.FindByClienteAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"El cliente {clienteId} no tiene una cuenta de millas registrada.");

        // 2. El aggregate MilesCuenta valida que haya saldo suficiente
        //    Si no alcanza, lanza InvalidOperationException con mensaje claro
        cuenta.RedimirMiles(millasARedimir);

        // 3. Crear el registro contable
        var movimiento = MilesMovimiento.PorRedencionEnReserva(
            cuenta.Id, reservaId, millasARedimir);

        // 4. Persistir
        await _movimientoRepo.SaveAsync(movimiento);
        await _cuentaRepo.UpdateAsync(cuenta);

        return movimiento;
    }
}
