// src/UI/Admin/Reportes/ReportesMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.flighthistory.Application.UseCases;
using AirTicketSystem.modules.milescuenta.Application.UseCases;
using AirTicketSystem.modules.milesmovimiento.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reportes;

public sealed class ReportesMenu
{
    private readonly IServiceProvider _provider;

    public ReportesMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Reportes LINQ");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un reporte",
                [
                    "8.1 Vuelos con mayor ocupación",
                    "8.2 Vuelos con asientos disponibles",
                    "8.3 Clientes con más reservas",
                    "8.4 Destinos más solicitados",
                    "8.5 Reservas por estado",
                    "8.6 Ingresos estimados por aerolínea",
                    "8.7 Historial de cambios por rango de fechas",
                    "── PROGRAMA DE MILLAS ──────────────────────",
                    "8.8 Clientes con más millas acumuladas",
                    "8.9 Clientes que más redimen millas",
                    "8.10 Aerolíneas con mayor volumen de fidelización",
                    "8.11 Rutas con mayor acumulación de millas",
                    "8.12 Ranking de viajeros frecuentes",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "8.1 Vuelos con mayor ocupación":        await Reporte1Async(); break;
                case "8.2 Vuelos con asientos disponibles":   await Reporte2Async(); break;
                case "8.3 Clientes con más reservas":         await Reporte3Async(); break;
                case "8.4 Destinos más solicitados":          await Reporte4Async(); break;
                case "8.5 Reservas por estado":               await Reporte5Async(); break;
                case "8.6 Ingresos estimados por aerolínea":  await Reporte6Async(); break;
                case "8.7 Historial de cambios por rango de fechas": await Reporte7Async(); break;
                case "8.8 Clientes con más millas acumuladas":         await Reporte8Async(); break;
                case "8.9 Clientes que más redimen millas":            await Reporte9Async(); break;
                case "8.10 Aerolíneas con mayor volumen de fidelización": await Reporte10Async(); break;
                case "8.11 Rutas con mayor acumulación de millas":     await Reporte11Async(); break;
                case "8.12 Ranking de viajeros frecuentes":            await Reporte12Async(); break;
                case "Volver": return;
            }
        }
    }

    // ── 8.1 Vuelos con mayor ocupación ───────────────────────────────────────
    private async Task Reporte1Async()
    {
        SpectreHelper.MostrarSubtitulo("8.1 — Vuelos con Mayor Ocupación");
        var top = SpectreHelper.PedirEntero("Mostrar top N vuelos (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos     = await scope.ServiceProvider.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var availUc    = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            // LINQ: para cada vuelo obtener asientos disponibles y calcular ocupación
            var resultado = new List<(int vueloId, string numero, string estado, int disponibles)>();
            foreach (var v in vuelos)
            {
                var disponibles = await availUc.ExecuteAsync(v.Id);
                resultado.Add((v.Id, v.NumeroVuelo.Valor, v.Estado.Valor, disponibles.Count));
            }

            var reporte = resultado
                .Where(r => r.estado != "CANCELADO")
                .OrderBy(r => r.disponibles)   // menos disponibles = más ocupado
                .Take(top)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de vuelos."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Estado", "Asientos disponibles");
            foreach (var (vueloId, numero, estado, disponibles) in reporte)
                SpectreHelper.AgregarFila(tabla, vueloId.ToString(), numero, estado, disponibles.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.2 Vuelos con asientos disponibles ──────────────────────────────────
    private async Task Reporte2Async()
    {
        SpectreHelper.MostrarSubtitulo("8.2 — Vuelos con Asientos Disponibles");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos  = await scope.ServiceProvider.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync();
            var availUc = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            var resultado = new List<(int id, string numero, string ruta, int disponibles)>();
            foreach (var v in vuelos)
            {
                var disponibles = await availUc.ExecuteAsync(v.Id);
                if (disponibles.Count > 0)
                    resultado.Add((v.Id, v.NumeroVuelo.Valor, $"Ruta #{v.RutaId}", disponibles.Count));
            }

            // LINQ: ordenar por más asientos disponibles
            var reporte = resultado
                .OrderByDescending(r => r.disponibles)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos con asientos disponibles."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Ruta", "Asientos disponibles");
            foreach (var (id, numero, ruta, disponibles) in reporte)
                SpectreHelper.AgregarFila(tabla, id.ToString(), numero, ruta, disponibles.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.3 Clientes con más reservas ────────────────────────────────────────
    private async Task Reporte3Async()
    {
        SpectreHelper.MostrarSubtitulo("8.3 — Clientes con Más Reservas");
        var top = SpectreHelper.PedirEntero("Mostrar top N clientes (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var clientes   = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var bookingUc  = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            var datos = new List<(int clienteId, int personaId, int reservas)>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                datos.Add((c.Id, c.PersonaId, reservas.Count));
            }

            // LINQ: ordenar por cantidad de reservas descendente
            var reporte = datos
                .OrderByDescending(d => d.reservas)
                .Take(top)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de clientes."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ClienteID", "PersonaID", "Total reservas");
            foreach (var (clienteId, personaId, reservas) in reporte)
                SpectreHelper.AgregarFila(tabla, clienteId.ToString(), personaId.ToString(), reservas.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.4 Destinos más solicitados ─────────────────────────────────────────
    private async Task Reporte4Async()
    {
        SpectreHelper.MostrarSubtitulo("8.4 — Destinos Más Solicitados");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope  = _provider.CreateAsyncScope();
            var rutas    = await scope.ServiceProvider.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync();
            var vueloUc  = scope.ServiceProvider.GetRequiredService<GetFlightsByRouteUseCase>();

            var conteos = new List<(int aeropuertoDestino, int rutaId, int vuelos)>();
            foreach (var r in rutas)
            {
                var vuelos = await vueloUc.ExecuteAsync(r.Id);
                conteos.Add((r.DestinoId, r.Id, vuelos.Count));
            }

            // LINQ: agrupar por aeropuerto destino, sumar vuelos, ordenar descendente
            var reporte = conteos
                .GroupBy(c => c.aeropuertoDestino)
                .Select(g => new
                {
                    AeropuertoDestinoID = g.Key,
                    TotalRutas          = g.Count(),
                    TotalVuelos         = g.Sum(x => x.vuelos)
                })
                .OrderByDescending(x => x.TotalVuelos)
                .ToList();

            // DestinoId es la propiedad correcta del aggregate Route

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de rutas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("AeropuertoDestinoID", "Total rutas", "Total vuelos");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.AeropuertoDestinoID.ToString(), r.TotalRutas.ToString(), r.TotalVuelos.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.5 Reservas por estado ───────────────────────────────────────────────
    private async Task Reporte5Async()
    {
        SpectreHelper.MostrarSubtitulo("8.5 — Reservas por Estado");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var clientes  = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var bookingUc = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            var todasReservas = new List<string>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                todasReservas.AddRange(reservas.Select(r => r.Estado.Valor));
            }

            // LINQ: agrupar por estado y contar
            var reporte = todasReservas
                .GroupBy(estado => estado)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin reservas registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("Estado", "Total reservas");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.Estado, r.Total.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.6 Ingresos estimados por aerolínea ─────────────────────────────────
    private async Task Reporte6Async()
    {
        SpectreHelper.MostrarSubtitulo("8.6 — Ingresos Estimados por Aerolínea");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope    = _provider.CreateAsyncScope();
            var aerolineas  = await scope.ServiceProvider.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync();
            var rutasUc     = scope.ServiceProvider.GetRequiredService<GetRoutesByAirlineUseCase>();
            var vuelosUc    = scope.ServiceProvider.GetRequiredService<GetFlightsByRouteUseCase>();
            var clientesUc  = scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>();
            var bookingUc   = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            // Obtener todas las reservas confirmadas
            var clientes = await clientesUc.ExecuteAsync();
            var reservasConVuelo = new List<(int vueloId, decimal total)>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                foreach (var r in reservas.Where(r => r.Estado.Valor == "CONFIRMADA"))
                    reservasConVuelo.Add((r.VueloId, r.ValorTotal.Valor));
            }

            var resultado = new List<(int aerolineaId, string nombre, decimal ingresos, int vuelos)>();
            foreach (var a in aerolineas)
            {
                var rutas = await rutasUc.ExecuteAsync(a.Id);
                var vueloIds = new List<int>();
                foreach (var r in rutas)
                {
                    var vuelos = await vuelosUc.ExecuteAsync(r.Id);
                    vueloIds.AddRange(vuelos.Select(v => v.Id));
                }

                // LINQ: suma de ingresos de reservas confirmadas de vuelos de esta aerolínea
                var ingresos = reservasConVuelo
                    .Where(rv => vueloIds.Contains(rv.vueloId))
                    .Sum(rv => rv.total);

                resultado.Add((a.Id, a.Nombre.Valor, ingresos, vueloIds.Count));
            }

            var reporte = resultado
                .OrderByDescending(r => r.ingresos)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de aerolíneas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("AerolineaID", "Nombre", "Total vuelos", "Ingresos estimados");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.aerolineaId.ToString(), r.nombre, r.vuelos.ToString(), r.ingresos.ToString("C"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.7 Historial de cambios de vuelo por rango de fechas ────────────────
    private async Task Reporte7Async()
    {
        SpectreHelper.MostrarSubtitulo("8.7 — Historial de Cambios por Rango de Fechas");
        var desdeStr = SpectreHelper.PedirTexto("Fecha desde (yyyy-MM-dd)");
        var hastaStr = SpectreHelper.PedirTexto("Fecha hasta (yyyy-MM-dd)");

        if (!DateTime.TryParse(desdeStr, out var desde) || !DateTime.TryParse(hastaStr, out var hasta))
        {
            SpectreHelper.MostrarError("Fechas inválidas."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope  = _provider.CreateAsyncScope();
            var vuelos    = await scope.ServiceProvider.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var histUc    = scope.ServiceProvider.GetRequiredService<GetFlightHistoryUseCase>();

            var todosLogs = new List<(int vueloId, string numero, string anterior, string nuevo, DateTime fecha, string? motivo)>();
            foreach (var v in vuelos)
            {
                var historial = await histUc.ExecuteAsync(v.Id);
                foreach (var h in historial)
                    todosLogs.Add((v.Id, v.NumeroVuelo.Valor, h.EstadoAnterior.Valor, h.EstadoNuevo.Valor, h.FechaCambio, h.Motivo?.Valor));
            }

            // LINQ: filtrar por rango de fechas y ordenar por fecha
            var reporte = todosLogs
                .Where(l => l.fecha >= desde && l.fecha <= hasta.AddDays(1))
                .OrderByDescending(l => l.fecha)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin cambios en el rango indicado."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Antes", "Después", "Fecha", "Motivo");
            foreach (var (vueloId, numero, anterior, nuevo, fecha, motivo) in reporte)
                SpectreHelper.AgregarFila(tabla,
                    vueloId.ToString(), numero, anterior, nuevo,
                    fecha.ToString("yyyy-MM-dd HH:mm"),
                    motivo ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ════════════════════════════════════════════════════════════════
    //   REPORTES DE MILLAS (8.8 – 8.12)
    // ════════════════════════════════════════════════════════════════

    // ── 8.8 Clientes con más millas acumuladas ───────────────────────────────
    private async Task Reporte8Async()
    {
        SpectreHelper.MostrarSubtitulo("8.8 — Clientes con Más Millas Acumuladas");
        var top = SpectreHelper.PedirEntero("Mostrar top N clientes (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var cuentas = await scope.ServiceProvider
                .GetRequiredService<GetAllCuentasMilesUseCase>()
                .ExecuteAsync();

            // LINQ: ordenar por total histórico acumulado, tomar los N primeros
            var reporte = cuentas
                .OrderByDescending(c => c.MilesAcumuladasTotal)
                .Take(top)
                .Select(c => new
                {
                    c.ClienteId,
                    SaldoDisponible  = c.SaldoActual.Valor,
                    c.MilesAcumuladasTotal,
                    Nivel            = c.Nivel.Valor,
                    Desde            = c.FechaInscripcion.Valor.ToString("yyyy-MM-dd")
                })
                .ToList();

            if (reporte.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin cuentas de millas registradas.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "ClienteID", "Saldo disponible", "Total acumulado", "Nivel", "Miembro desde");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla,
                    r.ClienteId.ToString(),
                    $"{r.SaldoDisponible:N0}",
                    $"{r.MilesAcumuladasTotal:N0}",
                    r.Nivel,
                    r.Desde);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.9 Clientes que más redimen millas ──────────────────────────────────
    private async Task Reporte9Async()
    {
        SpectreHelper.MostrarSubtitulo("8.9 — Clientes que Más Redimen Millas");
        var top = SpectreHelper.PedirEntero("Mostrar top N clientes (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            var movimientos = await sp.GetRequiredService<GetAllMovimientosUseCase>().ExecuteAsync();
            var cuentas     = await sp.GetRequiredService<GetAllCuentasMilesUseCase>().ExecuteAsync();

            // Diccionario para lookup rápido: CuentaId → ClienteId
            var cuentaACliente = cuentas.ToDictionary(c => c.Id, c => c.ClienteId);

            // LINQ: filtrar redenciones, agrupar por cuenta, sumar millas y contar transacciones
            var reporte = movimientos
                .Where(m => m.Tipo.EsRedencion)
                .GroupBy(m => m.CuentaId)
                .Select(g => new
                {
                    CuentaId       = g.Key,
                    TotalRedimido  = g.Sum(m => m.Millas.Valor),
                    NumRedenciones = g.Count()
                })
                .OrderByDescending(x => x.TotalRedimido)
                .Take(top)
                .Select(x => new
                {
                    ClienteId      = cuentaACliente.TryGetValue(x.CuentaId, out var cid) ? cid : 0,
                    x.TotalRedimido,
                    x.NumRedenciones,
                    PromedioXRed   = x.NumRedenciones > 0 ? x.TotalRedimido / x.NumRedenciones : 0
                })
                .ToList();

            if (reporte.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin redenciones registradas.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "ClienteID", "Total redimido", "Nº redenciones", "Promedio/redención");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla,
                    r.ClienteId.ToString(),
                    $"{r.TotalRedimido:N0} millas",
                    r.NumRedenciones.ToString(),
                    $"{r.PromedioXRed:N0} millas");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.10 Aerolíneas con mayor volumen de fidelización ────────────────────
    private async Task Reporte10Async()
    {
        SpectreHelper.MostrarSubtitulo("8.10 — Aerolíneas con Mayor Volumen de Fidelización");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            // Cargar todo en memoria para el join LINQ
            var movimientos = await sp.GetRequiredService<GetAllMovimientosUseCase>().ExecuteAsync();
            var aerolineas  = await sp.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync();
            var rutas       = await sp.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync();
            var vuelos      = await sp.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var reservas    = await CargarTodasLasReservasAsync(sp);

            // Construir diccionarios para joins eficientes
            var reservaAVuelo = reservas.ToDictionary(r => r.Id, r => r.VueloId);
            var vueloARuta    = vuelos.ToDictionary(v => v.Id, v => v.RutaId);
            var rutaAerolinea = rutas.ToDictionary(r => r.Id, r => r.AerolineaId);
            var nombreAerolinea = aerolineas.ToDictionary(a => a.Id, a => a.Nombre.Valor);

            // LINQ: acumulaciones → reserva → vuelo → ruta → aerolínea
            var reporte = movimientos
                .Where(m => m.Tipo.EsAcumulacion && m.ReservaId.HasValue)
                .Where(m => reservaAVuelo.ContainsKey(m.ReservaId!.Value))
                .Select(m => new
                {
                    Millas  = m.Millas.Valor,
                    VueloId = reservaAVuelo[m.ReservaId!.Value]
                })
                .Where(x => vueloARuta.ContainsKey(x.VueloId))
                .Select(x => new
                {
                    x.Millas,
                    RutaId = vueloARuta[x.VueloId]
                })
                .Where(x => rutaAerolinea.ContainsKey(x.RutaId))
                .Select(x => new
                {
                    x.Millas,
                    AerolineaId = rutaAerolinea[x.RutaId]
                })
                .GroupBy(x => x.AerolineaId)
                .Select(g => new
                {
                    AerolineaId    = g.Key,
                    Nombre         = nombreAerolinea.TryGetValue(g.Key, out var n) ? n : $"ID {g.Key}",
                    TotalMillas    = g.Sum(x => x.Millas),
                    NumMovimientos = g.Count()
                })
                .OrderByDescending(x => x.TotalMillas)
                .ToList();

            if (reporte.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin datos de fidelización por aerolínea.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "AerolineaID", "Nombre", "Millas distribuidas", "Nº acumulaciones");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla,
                    r.AerolineaId.ToString(),
                    r.Nombre,
                    $"{r.TotalMillas:N0}",
                    r.NumMovimientos.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.11 Rutas con mayor acumulación de millas ───────────────────────────
    private async Task Reporte11Async()
    {
        SpectreHelper.MostrarSubtitulo("8.11 — Rutas con Mayor Acumulación de Millas");
        var top = SpectreHelper.PedirEntero("Mostrar top N rutas (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            var movimientos = await sp.GetRequiredService<GetAllMovimientosUseCase>().ExecuteAsync();
            var rutas       = await sp.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync();
            var vuelos      = await sp.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var reservas    = await CargarTodasLasReservasAsync(sp);

            var reservaAVuelo = reservas.ToDictionary(r => r.Id, r => r.VueloId);
            var vueloARuta    = vuelos.ToDictionary(v => v.Id, v => v.RutaId);

            // Enriquecer ruta con origen/destino para display
            var infoRuta = rutas.ToDictionary(
                r => r.Id,
                r => $"Ruta #{r.Id} (O:{r.OrigenId} → D:{r.DestinoId})");

            // LINQ: acumulaciones → reserva → vuelo → ruta
            var reporte = movimientos
                .Where(m => m.Tipo.EsAcumulacion && m.ReservaId.HasValue)
                .Where(m => reservaAVuelo.ContainsKey(m.ReservaId!.Value))
                .Select(m => new
                {
                    Millas  = m.Millas.Valor,
                    VueloId = reservaAVuelo[m.ReservaId!.Value]
                })
                .Where(x => vueloARuta.ContainsKey(x.VueloId))
                .Select(x => new
                {
                    x.Millas,
                    RutaId = vueloARuta[x.VueloId]
                })
                .GroupBy(x => x.RutaId)
                .Select(g => new
                {
                    RutaId        = g.Key,
                    Info          = infoRuta.TryGetValue(g.Key, out var i) ? i : $"Ruta #{g.Key}",
                    TotalMillas   = g.Sum(x => x.Millas),
                    TotalVuelos   = g.Select(x => x.RutaId).Distinct().Count()
                })
                .OrderByDescending(x => x.TotalMillas)
                .Take(top)
                .ToList();

            if (reporte.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin datos de acumulación por ruta.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("RutaID", "Ruta", "Millas acumuladas");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla,
                    r.RutaId.ToString(),
                    r.Info,
                    $"{r.TotalMillas:N0}");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.12 Ranking de viajeros frecuentes ──────────────────────────────────
    private async Task Reporte12Async()
    {
        SpectreHelper.MostrarSubtitulo("8.12 — Ranking de Viajeros Frecuentes");
        var top = SpectreHelper.PedirEntero("Mostrar top N viajeros (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;

            var cuentas  = await sp.GetRequiredService<GetAllCuentasMilesUseCase>().ExecuteAsync();
            var reservas = await CargarTodasLasReservasAsync(sp);

            // Agrupar reservas confirmadas por cliente para contar vuelos completados
            var vuelosPorCliente = reservas
                .Where(r => r.Estado.Valor == "CONFIRMADA")
                .GroupBy(r => r.ClienteId)
                .ToDictionary(g => g.Key, g => g.Count());

            // LINQ multi-criterio:
            //   1º millas acumuladas históricas (fidelidad total)
            //   2º vuelos confirmados (actividad)
            //   3º saldo disponible (engagement actual)
            var reporte = cuentas
                .Select(c => new
                {
                    c.ClienteId,
                    Nivel              = c.Nivel.Valor,
                    MilesTotal         = c.MilesAcumuladasTotal,
                    SaldoDisponible    = c.SaldoActual.Valor,
                    VuelosConfirmados  = vuelosPorCliente.TryGetValue(c.ClienteId, out var v) ? v : 0,
                    Desde              = c.FechaInscripcion.Valor.ToString("yyyy-MM-dd")
                })
                .OrderByDescending(x => x.MilesTotal)
                .ThenByDescending(x => x.VuelosConfirmados)
                .ThenByDescending(x => x.SaldoDisponible)
                .Take(top)
                .ToList();

            if (reporte.Count == 0)
            {
                SpectreHelper.MostrarInfo("Sin viajeros frecuentes registrados.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "#", "ClienteID", "Nivel", "Miles total", "Saldo", "Vuelos", "Miembro desde");
            var posicion = 1;
            foreach (var r in reporte)
            {
                SpectreHelper.AgregarFila(tabla,
                    posicion++.ToString(),
                    r.ClienteId.ToString(),
                    r.Nivel,
                    $"{r.MilesTotal:N0}",
                    $"{r.SaldoDisponible:N0}",
                    r.VuelosConfirmados.ToString(),
                    r.Desde);
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── Helper: carga todas las reservas iterando por todos los clientes ──────
    // Sigue el mismo patrón de los reportes 8.5 y 8.6 existentes.
    private async Task<List<AirTicketSystem.modules.booking.Domain.aggregate.Booking>>
        CargarTodasLasReservasAsync(IServiceProvider sp)
    {
        var clientes = await sp.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
        var bookingUc = sp.GetRequiredService<GetBookingsByClienteUseCase>();
        var todas = new List<AirTicketSystem.modules.booking.Domain.aggregate.Booking>();
        foreach (var c in clientes)
        {
            var reservas = await bookingUc.ExecuteAsync(c.Id);
            todas.AddRange(reservas);
        }
        return todas;
    }
}
