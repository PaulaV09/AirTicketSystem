// src/UI/Client/MyBookingsMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.ticket.Application.UseCases;
using AirTicketSystem.modules.boardingpass.Application.UseCases;
using AirTicketSystem.modules.checkin.Application.UseCases;
using AirTicketSystem.modules.payment.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.luggage.Application.UseCases;

namespace AirTicketSystem.UI.Client;

public sealed class MyBookingsMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public MyBookingsMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mis Reservas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "2.1 Ver mis reservas",
                    "2.2 Detalle de reserva",
                    "2.3 Cancelar mi reserva",
                    "2.4 Hacer check-in virtual",
                    "2.5 Mi pase de abordar",
                    "2.6 Gestionar equipaje",
                    "2.7 Ver mis tiquetes",
                    "Pasajeros de mi reserva",
                    "Mis pagos",
                    "Emitir tiquete",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "2.1 Ver mis reservas":        await VerReservasAsync();          break;
                case "2.2 Detalle de reserva":      await DetalleReservaAsync();       break;
                case "2.3 Cancelar mi reserva":     await CancelarReservaAsync();      break;
                case "2.4 Hacer check-in virtual":  await HacerCheckinVirtualAsync();  break;
                case "2.5 Mi pase de abordar":      await VerPaseAbordarAsync();       break;
                case "2.6 Gestionar equipaje":      await GestionarEquipajeAsync();    break;
                case "2.7 Ver mis tiquetes":        await ConsultarTiqueteAsync();     break;
                case "Pasajeros de mi reserva":     await VerPasajerosAsync();         break;
                case "Mis pagos":                   await VerPagosAsync();             break;
                case "Emitir tiquete":              await VerTiquetesAsync();          break;
                case "Volver":                      return;
            }
        }
    }

    private async Task VerReservasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var clienteId = await ObtenerClienteIdAsync();
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>()
                .ExecuteAsync(clienteId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No tiene reservas registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Código", "VueloID", "Estado", "Total", "Expira");
            foreach (var b in lista)
                SpectreHelper.AgregarFila(tabla,
                    b.Id.ToString(), b.CodigoReserva.Valor,
                    b.VueloId.ToString(), b.Estado.Valor,
                    b.ValorTotal.Valor.ToString("C2"),
                    b.FechaExpiracion.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task DetalleReservaAsync()
    {
        var codigo = SpectreHelper.PedirTexto("Código de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<GetBookingByCodigoUseCase>()
                .ExecuteAsync(codigo);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",            b.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Código",        b.CodigoReserva.Valor);
            SpectreHelper.AgregarFila(tabla, "VueloID",       b.VueloId.ToString());
            SpectreHelper.AgregarFila(tabla, "TarifaID",      b.TarifaId.ToString());
            SpectreHelper.AgregarFila(tabla, "Estado",        b.Estado.Valor);
            SpectreHelper.AgregarFila(tabla, "Total",         b.ValorTotal.Valor.ToString("C2"));
            SpectreHelper.AgregarFila(tabla, "Fecha reserva", b.FechaReserva.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.AgregarFila(tabla, "Expira",        b.FechaExpiracion.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.AgregarFila(tabla, "Observaciones", b.Observaciones?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPasajerosAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetPassengersByBookingUseCase>()
                .ExecuteAsync(reservaId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin pasajeros en esa reserva."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "Tipo", "AsientoID");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.PersonaId.ToString(),
                    p.TipoPasajero.Valor,
                    p.AsientoId?.ToString() ?? "Sin asignar");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CancelarReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva a cancelar");
        var motivo    = SpectreHelper.PedirTexto("Motivo de cancelación");
        if (!SpectreHelper.Confirmar("¿Confirma cancelar la reserva?")) { SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<CancelBookingUseCase>()
                .ExecuteAsync(reservaId, motivo, _session.CurrentUserId > 0 ? _session.CurrentUserId : null);
            SpectreHelper.MostrarExito($"Reserva [{b.CodigoReserva.Valor}] cancelada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task VerPagosAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetPaymentsByBookingUseCase>()
                .ExecuteAsync(reservaId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin pagos para esa reserva."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "MetodoPagoID", "Monto", "Estado", "Referencia", "FechaPago");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.MetodoPagoId.ToString(),
                    p.Monto.Valor.ToString("C2"), p.Estado.Valor,
                    p.Referencia?.Valor ?? "-",
                    p.FechaPago?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerTiquetesAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<EmitTicketUseCase>()
                .ExecuteAsync(pasajeroReservaId);
            SpectreHelper.MostrarExito(
                $"Tiquete emitido.\n" +
                $"  Código  : {t.CodigoTiquete.Valor}\n" +
                $"  Estado  : {t.Estado.Valor}\n" +
                $"  Emitido : {t.FechaEmision.Valor:yyyy-MM-dd HH:mm}");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ConsultarTiqueteAsync()
    {
        var codigo = SpectreHelper.PedirTexto("Código del tiquete");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<GetTicketByCodeUseCase>()
                .ExecuteAsync(codigo);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",              t.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Código",          t.CodigoTiquete.Valor);
            SpectreHelper.AgregarFila(tabla, "PasajeroReservaID", t.PasajeroReservaId.ToString());
            SpectreHelper.AgregarFila(tabla, "AsientoID",       t.AsientoConfirmadoId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Estado",          t.Estado.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha emisión",   t.FechaEmision.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerPaseAbordarAsync()
    {
        var codigo = SpectreHelper.PedirTexto("Código del pase de abordar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var bp = await scope.ServiceProvider.GetRequiredService<GetBoardingPassByCodeUseCase>()
                .ExecuteAsync(codigo);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",           bp.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Código pase",  bp.CodigoPase.Valor);
            SpectreHelper.AgregarFila(tabla, "Código QR",    bp.CodigoQr.Valor);
            SpectreHelper.AgregarFila(tabla, "CheckinID",    bp.CheckinId.ToString());
            SpectreHelper.AgregarFila(tabla, "PuertaID",     bp.PuertaEmbarqueId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Hora embarque", bp.HoraEmbarque?.Valor.ToString("HH:mm") ?? "-");
            SpectreHelper.AgregarFila(tabla, "Fecha emisión", bp.FechaEmision.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 2.4 Hacer check-in virtual ───────────────────────────────────────────
    private async Task HacerCheckinVirtualAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CreateVirtualCheckInUseCase>()
                .ExecuteAsync(pasajeroReservaId);
            SpectreHelper.MostrarExito($"Check-in virtual realizado (ID {c.Id}). Estado: {c.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    // ── 2.6 Gestionar equipaje ───────────────────────────────────────────────
    private async Task GestionarEquipajeAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mi Equipaje");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Acción",
                ["Ver mi equipaje", "Agregar equipaje", "Volver"]);
            if (opcion == "Volver") return;

            await ConsoleErrorHandler.ExecuteAsync(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                switch (opcion)
                {
                    case "Ver mi equipaje":
                        var pasajeroId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
                        var lista = await scope.ServiceProvider.GetRequiredService<GetLuggageByPassengerUseCase>()
                            .ExecuteAsync(pasajeroId);
                        if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin equipaje registrado."); SpectreHelper.EsperarTecla(); return; }
                        var tabla = SpectreHelper.CrearTabla("ID", "Tipo", "Peso(kg)", "Estado");
                        foreach (var l in lista)
                            SpectreHelper.AgregarFila(tabla,
                                l.Id.ToString(), l.TipoEquipajeId.ToString(),
                                l.PesoDeclaradoKg?.Valor.ToString("F1") ?? "-",
                                l.Estado.Valor);
                        SpectreHelper.MostrarTabla(tabla);
                        SpectreHelper.EsperarTecla();
                        break;

                    case "Agregar equipaje":
                        var pId   = SpectreHelper.PedirEntero("ID del pasajero-reserva");
                        var vId   = SpectreHelper.PedirEntero("ID del vuelo");
                        var tId   = SpectreHelper.PedirEntero("ID del tipo de equipaje");
                        var desc  = SpectreHelper.PedirTexto("Descripción (opcional)");
                        var pesoS = SpectreHelper.PedirTexto("Peso en kg (opcional)");
                        string? dOpc = string.IsNullOrWhiteSpace(desc) ? null : desc;
                        decimal? peso = decimal.TryParse(pesoS, out var p) ? p : null;
                        var lug = await scope.ServiceProvider.GetRequiredService<RegisterLuggageUseCase>()
                            .ExecuteAsync(pId, vId, tId, dOpc, peso);
                        SpectreHelper.MostrarExito($"Equipaje registrado (ID {lug.Id}).");
                        SpectreHelper.EsperarTecla();
                        break;
                }
            });
        }
    }

    private async Task<int> ObtenerClienteIdAsync()
    {
        await using var scope = _provider.CreateAsyncScope();
        var clientes = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>()
            .ExecuteAsync();
        return clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)?.Id
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente. Contacte al administrador.");
    }
}
