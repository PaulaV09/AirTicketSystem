// src/modules/milesmovimiento/Application/UseCases/RegistrarAcumulacionUseCase.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milesmovimiento.Application.UseCases;

// Este use case coordina dos aggregates:
//   1. MilesCuenta  → actualiza el saldo y nivel del cliente
//   2. MilesMovimiento → registra el evento contable inmutable
// La coordinación ocurre aquí (Application), nunca en el Domain.
public sealed class RegistrarAcumulacionUseCase
{
    private readonly IMilesMovimientoRepository _movimientoRepo;
    private readonly IMilesCuentaRepository _cuentaRepo;

    public RegistrarAcumulacionUseCase(
        IMilesMovimientoRepository movimientoRepo,
        IMilesCuentaRepository cuentaRepo)
    {
        _movimientoRepo = movimientoRepo;
        _cuentaRepo     = cuentaRepo;
    }

    // Regla de acumulación: 1 milla por cada $1.000 COP del valor de la reserva.
    public async Task<MilesMovimiento> ExecuteAsync(
        int clienteId,
        int reservaId,
        decimal valorReserva,
        string numeroVuelo,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        if (valorReserva <= 0)
            throw new ArgumentException("El valor de la reserva debe ser mayor a 0.");

        // Calcular millas: 1 milla por $1.000 COP (mínimo 1 milla)
        var millas = Math.Max(1, (int)(valorReserva / 1_000m));

        // 1. Obtener la cuenta del cliente
        var cuenta = await _cuentaRepo.FindByClienteAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"El cliente {clienteId} no tiene una cuenta de millas. " +
                "Debe crear la cuenta antes de acumular millas.");

        // 2. Aplicar la acumulación en el aggregate MilesCuenta
        cuenta.AcumularMiles(millas);

        // 3. Crear el registro contable en MilesMovimiento
        var movimiento = MilesMovimiento.PorVueloCompletado(
            cuenta.Id, reservaId, millas, numeroVuelo);

        // 4. Persistir los cambios (primero el movimiento, luego actualizar cuenta)
        await _movimientoRepo.SaveAsync(movimiento);
        await _cuentaRepo.UpdateAsync(cuenta);

        return movimiento;
    }
}
