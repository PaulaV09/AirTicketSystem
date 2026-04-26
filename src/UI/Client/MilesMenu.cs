// src/UI/Client/MilesMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.payment.Application.UseCases;
using AirTicketSystem.modules.paymentmethod.Application.UseCases;
using AirTicketSystem.modules.milescuenta.Application.UseCases;
using AirTicketSystem.modules.milesmovimiento.Application.UseCases;

namespace AirTicketSystem.UI.Client;

public sealed class MilesMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public MilesMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Millas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una opción",
                [
                    "4.1 Mi saldo y nivel",
                    "4.2 Historial de movimientos",
                    "4.3 Redimir millas en una reserva",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "4.1 Mi saldo y nivel":              await VerSaldoAsync();    break;
                case "4.2 Historial de movimientos":      await VerHistorialAsync(); break;
                case "4.3 Redimir millas en una reserva": await RedimirAsync();      break;
                case "Volver":                            return;
            }
        }
    }

    // ── 4.1 Saldo y nivel ────────────────────────────────────────────────────
    private async Task VerSaldoAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var clienteId = await ObtenerClienteIdAsync();
            await using var scope = _provider.CreateAsyncScope();

            var cuenta = await scope.ServiceProvider
                .GetRequiredService<GetCuentaMilesByClienteUseCase>()
                .ExecuteAsync(clienteId);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "Saldo disponible",    $"{cuenta.SaldoActual.Valor:N0} millas");
            SpectreHelper.AgregarFila(tabla, "Millas acumuladas (total histórico)", $"{cuenta.MilesAcumuladasTotal:N0} millas");
            SpectreHelper.AgregarFila(tabla, "Nivel",               cuenta.Nivel.Valor);
            SpectreHelper.AgregarFila(tabla, "Equivalente en pesos", $"${cuenta.SaldoActual.Valor:N0} COP");
            SpectreHelper.AgregarFila(tabla, "Miembro desde",       cuenta.FechaInscripcion.Valor.ToString("yyyy-MM-dd"));

            if (!cuenta.Nivel.EsPlatino)
                SpectreHelper.AgregarFila(tabla, "Millas para siguiente nivel",
                    $"{cuenta.MilesParaSiguienteNivel:N0} millas");

            SpectreHelper.MostrarTabla(tabla);
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 4.2 Historial ────────────────────────────────────────────────────────
    private async Task VerHistorialAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var clienteId = await ObtenerClienteIdAsync();
            await using var scope = _provider.CreateAsyncScope();

            var movimientos = await scope.ServiceProvider
                .GetRequiredService<GetMovimientosByClienteUseCase>()
                .ExecuteAsync(clienteId);

            if (movimientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No tiene movimientos de millas registrados.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("ID", "Tipo", "Millas", "Fecha", "Descripción");
            foreach (var m in movimientos)
                SpectreHelper.AgregarFila(tabla,
                    m.Id.ToString(),
                    m.Tipo.Valor,
                    (m.Tipo.EsAcumulacion ? "+" : "-") + $"{m.Millas.Valor:N0}",
                    m.Fecha.Valor.ToString("yyyy-MM-dd HH:mm"),
                    m.Descripcion.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 4.3 Redimir millas ───────────────────────────────────────────────────
    private async Task RedimirAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var clienteId = await ObtenerClienteIdAsync();
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            // Mostrar saldo disponible antes de pedir la redención
            var cuenta = await sp.GetRequiredService<GetCuentaMilesByClienteUseCase>()
                .ExecuteAsync(clienteId);

            SpectreHelper.MostrarInfo(
                $"Saldo disponible: {cuenta.SaldoActual.Valor:N0} millas " +
                $"(equivale a ${cuenta.SaldoActual.Valor:N0} COP)");

            if (!cuenta.TieneMiles)
            {
                SpectreHelper.MostrarError("No tiene millas disponibles para redimir.");
                SpectreHelper.EsperarTecla();
                return;
            }

            // Seleccionar reserva
            var reservas = await sp.GetRequiredService<GetBookingsByClienteUseCase>()
                .ExecuteAsync(clienteId);
            var reservasPendientes = reservas
                .Where(r => r.Estado.Valor is "PENDIENTE" or "CONFIRMADA")
                .ToList();

            if (reservasPendientes.Count == 0)
            {
                SpectreHelper.MostrarError("No tiene reservas activas en las que aplicar millas.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var reserva = SpectreHelper.SeleccionarOpcion(
                "Seleccione la reserva donde desea usar las millas",
                reservasPendientes,
                r => $"  [{r.CodigoReserva.Valor}]  Vuelo #{r.VueloId}  " +
                     $"Total: ${r.ValorTotal.Valor:N0}  Estado: {r.Estado.Valor}");

            // Cuántas millas redimir
            var millasStr = SpectreHelper.PedirTexto(
                $"¿Cuántas millas desea redimir? (máx. {cuenta.SaldoActual.Valor:N0})");
            if (!int.TryParse(millasStr, out var millas) || millas <= 0)
            {
                SpectreHelper.MostrarError("Cantidad de millas no válida.");
                SpectreHelper.EsperarTecla();
                return;
            }

            // Calcular desglose
            var descuento  = Math.Min(millas, cuenta.SaldoActual.Valor);
            var montoMiles = (decimal)descuento;          // 1 milla = $1 COP
            var saldoReserva = reserva.ValorTotal.Valor;
            var montoRestante = Math.Max(0, saldoReserva - montoMiles);

            SpectreHelper.MostrarSubtitulo("Resumen del pago");
            var resumen = SpectreHelper.CrearTabla("Concepto", "Valor");
            SpectreHelper.AgregarFila(resumen, "Valor total reserva",  $"${saldoReserva:N0} COP");
            SpectreHelper.AgregarFila(resumen, "Millas a redimir",     $"{descuento:N0} millas");
            SpectreHelper.AgregarFila(resumen, "Descuento en millas",  $"${montoMiles:N0} COP");
            SpectreHelper.AgregarFila(resumen, "Saldo a pagar en dinero", $"${montoRestante:N0} COP");
            SpectreHelper.MostrarTabla(resumen);

            if (!SpectreHelper.Confirmar("¿Confirma redimir estas millas?"))
            {
                SpectreHelper.EsperarTecla();
                return;
            }

            // Seleccionar método de pago (para el monto restante en dinero)
            int metodoPagoId;
            if (montoRestante > 0)
            {
                var metodo = await SelectorUI.SeleccionarMetodoPagoAsync(_provider);
                if (metodo is null) { SpectreHelper.EsperarTecla(); return; }
                metodoPagoId = metodo.Id;
            }
            else
            {
                // Pago 100% con millas: usamos el primer método disponible como referencia
                var metodos = await sp.GetRequiredService<GetAllPaymentMethodsUseCase>().ExecuteAsync();
                metodoPagoId = metodos.First().Id;
            }

            // Paso 1: Registrar la redención (descuenta saldo + crea movimiento contable)
            var movimiento = await sp.GetRequiredService<RegistrarRedencionUseCase>()
                .ExecuteAsync(clienteId, reserva.Id, descuento);

            // Paso 2: Crear el pago con las millas y el dinero restante
            var pago = await sp.GetRequiredService<CreatePaymentUseCase>()
                .ExecuteAsync(reserva.Id, metodoPagoId, montoRestante, descuento);

            SpectreHelper.MostrarExito(
                $"Millas redimidas exitosamente.\n" +
                $"  Movimiento ID : {movimiento.Id}\n" +
                $"  Millas usadas : {descuento:N0}\n" +
                $"  Pago ID       : {pago.Id}  —  Estado: {pago.Estado.Valor}\n" +
                $"  Total cubierto: ${pago.MontoTotalCubierto:N0} COP");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task<int> ObtenerClienteIdAsync()
    {
        await using var scope = _provider.CreateAsyncScope();
        var clientes = await scope.ServiceProvider
            .GetRequiredService<GetAllClientsUseCase>()
            .ExecuteAsync();
        return clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)?.Id
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente. Contacte al administrador.");
    }
}
