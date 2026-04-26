// src/UI/Admin/Miles/MilesAdminMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.milescuenta.Application.UseCases;
using AirTicketSystem.modules.milesmovimiento.Application.UseCases;
using AirTicketSystem.modules.flight.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Miles;

public sealed class MilesAdminMenu
{
    private readonly IServiceProvider _provider;

    public MilesAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Programa de Millas — Administración");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una opción",
                [
                    "9.1 Ver todas las cuentas de millas",
                    "9.2 Ver movimientos por cliente",
                    "9.3 Acumular millas por vuelo completado",
                    "9.4 Crear cuenta de millas manualmente",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "9.1 Ver todas las cuentas de millas":       await VerTodasLasCuentasAsync();   break;
                case "9.2 Ver movimientos por cliente":           await VerMovimientosAsync();        break;
                case "9.3 Acumular millas por vuelo completado":  await AcumularPorVueloAsync();      break;
                case "9.4 Crear cuenta de millas manualmente":    await CrearCuentaManualAsync();     break;
                case "Volver":                                    return;
            }
        }
    }

    // ── 9.1 Ver todas las cuentas ────────────────────────────────────────────
    private async Task VerTodasLasCuentasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var cuentas = await scope.ServiceProvider
                .GetRequiredService<GetAllCuentasMilesUseCase>()
                .ExecuteAsync();

            if (cuentas.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay cuentas de millas registradas.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "ID", "ClienteID", "Saldo", "Total histórico", "Nivel", "Miembro desde");
            foreach (var c in cuentas)
                SpectreHelper.AgregarFila(tabla,
                    c.Id.ToString(),
                    c.ClienteId.ToString(),
                    $"{c.SaldoActual.Valor:N0}",
                    $"{c.MilesAcumuladasTotal:N0}",
                    c.Nivel.Valor,
                    c.FechaInscripcion.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.MostrarTabla(tabla);
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 9.2 Ver movimientos por cliente ──────────────────────────────────────
    private async Task VerMovimientosAsync()
    {
        var clienteId = SpectreHelper.PedirEntero("ID del cliente");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            // Mostrar saldo primero
            var cuenta = await sp.GetRequiredService<GetCuentaMilesByClienteUseCase>()
                .ExecuteAsync(clienteId);
            SpectreHelper.MostrarInfo(
                $"Cliente {clienteId}  |  Saldo: {cuenta.SaldoActual.Valor:N0} millas  " +
                $"|  Nivel: {cuenta.Nivel.Valor}");

            var movimientos = await sp.GetRequiredService<GetMovimientosByClienteUseCase>()
                .ExecuteAsync(clienteId);

            if (movimientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin movimientos registrados para este cliente.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "ID", "Tipo", "Millas", "ReservaID", "Fecha", "Descripción");
            foreach (var m in movimientos)
                SpectreHelper.AgregarFila(tabla,
                    m.Id.ToString(),
                    m.Tipo.Valor,
                    (m.Tipo.EsAcumulacion ? "+" : "-") + $"{m.Millas.Valor:N0}",
                    m.ReservaId?.ToString() ?? "-",
                    m.Fecha.Valor.ToString("yyyy-MM-dd HH:mm"),
                    m.Descripcion.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 9.3 Acumular millas por vuelo ─────────────────────────────────────────
    private async Task AcumularPorVueloAsync()
    {
        SpectreHelper.MostrarSubtitulo("Acumulación automática de millas por vuelo completado");
        SpectreHelper.MostrarInfo("Solo se procesan reservas CONFIRMADAS de vuelos en estado ATERRIZADO.");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            // Mostrar vuelos aterrizados para facilitar la selección
            var vuelos = await sp.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var aterrizados = vuelos
                .Where(v => v.Estado.Valor == "ATERRIZADO")
                .OrderByDescending(v => v.FechaLlegadaReal)
                .ToList();

            if (aterrizados.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay vuelos en estado ATERRIZADO en este momento.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var vuelo = SpectreHelper.SeleccionarOpcion(
                "Seleccione el vuelo para acumular millas",
                aterrizados,
                v => $"  [{v.NumeroVuelo.Valor}]  ID: {v.Id}  " +
                     $"Salida: {v.FechaSalida.Valor:yyyy-MM-dd}  Estado: {v.Estado.Valor}");

            if (!SpectreHelper.Confirmar(
                    $"¿Confirma acumular millas para todos los pasajeros del vuelo {vuelo.NumeroVuelo.Valor}?"))
            {
                SpectreHelper.EsperarTecla();
                return;
            }

            var resumen = await sp.GetRequiredService<AcumularMilesPorVueloUseCase>()
                .ExecuteAsync(vuelo.Id);

            SpectreHelper.MostrarExito(
                $"Proceso completado para vuelo {resumen.NumeroVuelo}.\n" +
                $"  Reservas confirmadas : {resumen.ReservasConfirmadas}\n" +
                $"  Clientes procesados  : {resumen.ClientesProcesados}\n" +
                $"  Millas distribuidas  : {resumen.TotalMilesAcumuladas:N0}");

            if (resumen.TuvoAdvertencias)
            {
                SpectreHelper.MostrarInfo("Advertencias del proceso:");
                foreach (var adv in resumen.Advertencias)
                    SpectreHelper.MostrarInfo($"  • {adv}");
            }
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 9.4 Crear cuenta manualmente ─────────────────────────────────────────
    private async Task CrearCuentaManualAsync()
    {
        var clienteId = SpectreHelper.PedirEntero("ID del cliente");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var cuenta = await scope.ServiceProvider
                .GetRequiredService<CrearCuentaMilesUseCase>()
                .ExecuteAsync(clienteId);

            SpectreHelper.MostrarExito(
                $"Cuenta de millas creada (ID {cuenta.Id})  |  " +
                $"Cliente {cuenta.ClienteId}  |  Nivel: {cuenta.Nivel.Valor}");
        });
        SpectreHelper.EsperarTecla();
    }
}
