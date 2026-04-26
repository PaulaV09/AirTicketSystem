// src/modules/milesmovimiento/Application/UseCases/AcumularMilesPorVueloUseCase.cs
using AirTicketSystem.modules.milesmovimiento.Application.DTOs;
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.milesmovimiento.Application.UseCases;

// Regla de acumulación: 1 milla por cada $1.000 COP del valor de la reserva.
// Solo procesa reservas CONFIRMADAS del vuelo; el resto se ignora.
// Es idempotente: si ya se acumularon millas para una reserva, la omite.
public sealed class AcumularMilesPorVueloUseCase
{
    private readonly IFlightRepository          _flightRepo;
    private readonly IBookingRepository         _bookingRepo;
    private readonly IMilesCuentaRepository     _cuentaRepo;
    private readonly IMilesMovimientoRepository _movimientoRepo;

    public AcumularMilesPorVueloUseCase(
        IFlightRepository          flightRepo,
        IBookingRepository         bookingRepo,
        IMilesCuentaRepository     cuentaRepo,
        IMilesMovimientoRepository movimientoRepo)
    {
        _flightRepo     = flightRepo;
        _bookingRepo    = bookingRepo;
        _cuentaRepo     = cuentaRepo;
        _movimientoRepo = movimientoRepo;
    }

    public async Task<ResumenAcumulacionMiles> ExecuteAsync(
        int vueloId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        // ── 1. Verificar que el vuelo existe y está ATERRIZADO ──────────────
        var vuelo = await _flightRepo.FindByIdAsync(vueloId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el vuelo con ID {vueloId}.");

        if (vuelo.Estado.Valor != "ATERRIZADO")
            throw new InvalidOperationException(
                $"Las millas solo se acumulan en vuelos ATERRIZADO. " +
                $"Estado actual del vuelo {vuelo.NumeroVuelo}: '{vuelo.Estado}'.");

        // ── 2. Obtener reservas CONFIRMADAS del vuelo ───────────────────────
        var todasReservas = await _bookingRepo.FindByVueloAsync(vueloId);
        var reservasConfirmadas = todasReservas
            .Where(r => r.Estado.Valor == "CONFIRMADA")
            .ToList();

        // ── 3. Procesar cada reserva confirmada ─────────────────────────────
        int clientesProcesados    = 0;
        int totalMilesAcumuladas  = 0;
        var advertencias          = new List<string>();

        foreach (var reserva in reservasConfirmadas)
        {
            try
            {
                // Idempotencia: si ya se acumularon millas para esta reserva, omitir
                var yaAcumulado = await _movimientoRepo.ExisteAcumulacionByReservaAsync(reserva.Id);
                if (yaAcumulado)
                {
                    advertencias.Add(
                        $"Reserva #{reserva.Id} (Cliente {reserva.ClienteId}): " +
                        "ya tenía millas acumuladas — omitida.");
                    continue;
                }

                // ── 3a. Calcular millas: 1 por $1.000 COP (mínimo 1) ───────
                var millas = Math.Max(1, (int)(reserva.ValorTotal.Valor / 1_000m));

                // ── 3b. Obtener o crear la cuenta de millas del cliente ──────
                var cuenta = await _cuentaRepo.FindByClienteAsync(reserva.ClienteId);
                if (cuenta is null)
                {
                    // Creación automática: el cliente viajó pero no había abierto
                    // su cuenta; se crea para que no pierda las millas
                    cuenta = MilesCuenta.Crear(reserva.ClienteId);
                    await _cuentaRepo.SaveAsync(cuenta);
                    advertencias.Add(
                        $"Cliente {reserva.ClienteId}: cuenta de millas creada automáticamente.");
                }

                // ── 3c. Aplicar acumulación en el aggregate ─────────────────
                cuenta.AcumularMiles(millas);

                // ── 3d. Registrar el movimiento contable ────────────────────
                var movimiento = MilesMovimiento.PorVueloCompletado(
                    cuenta.Id,
                    reserva.Id,
                    millas,
                    vuelo.NumeroVuelo.Valor);

                // ── 3e. Persistir ────────────────────────────────────────────
                await _movimientoRepo.SaveAsync(movimiento);
                await _cuentaRepo.UpdateAsync(cuenta);

                clientesProcesados++;
                totalMilesAcumuladas += millas;
            }
            catch (Exception ex)
            {
                // Un error en un cliente no cancela el proceso completo
                advertencias.Add(
                    $"Reserva #{reserva.Id} (Cliente {reserva.ClienteId}): {ex.Message}");
            }
        }

        return new ResumenAcumulacionMiles(
            vueloId,
            vuelo.NumeroVuelo.Valor,
            reservasConfirmadas.Count,
            clientesProcesados,
            totalMilesAcumuladas,
            advertencias.AsReadOnly());
    }
}
