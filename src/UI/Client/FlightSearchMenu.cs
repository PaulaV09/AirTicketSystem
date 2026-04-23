// src/UI/Client/FlightSearchMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.fare.Application.UseCases;

namespace AirTicketSystem.UI.Client;

public sealed class FlightSearchMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public FlightSearchMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Buscar Vuelos y Reservar");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Buscar vuelos por origen, destino y fecha",
                    "Ver asientos disponibles de un vuelo",
                    "Crear reserva",
                    "Agregar pasajero a reserva",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Buscar vuelos por origen, destino y fecha": await BuscarVuelosAsync();      break;
                case "Ver asientos disponibles de un vuelo":      await VerAsientosAsync();       break;
                case "Crear reserva":                             await CrearReservaAsync();      break;
                case "Agregar pasajero a reserva":                await AgregarPasajeroAsync();   break;
                case "Volver":                                    return;
            }
        }
    }

    private async Task BuscarVuelosAsync()
    {
        SpectreHelper.MostrarSubtitulo("Buscar Vuelos");
        await MostrarAeropuertosAsync();

        var origenId  = SpectreHelper.PedirEntero("ID del aeropuerto de origen");
        var destinoId = SpectreHelper.PedirEntero("ID del aeropuerto de destino");
        var fechaStr  = SpectreHelper.PedirTexto("Fecha de viaje (yyyy-MM-dd)");

        if (!DateTime.TryParse(fechaStr, out var fecha))
        {
            SpectreHelper.MostrarError("Fecha inválida."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos = await scope.ServiceProvider.GetRequiredService<SearchFlightsUseCase>()
                .ExecuteAsync(origenId, destinoId, fecha);

            var tabla = SpectreHelper.CrearTabla("ID", "Número", "Salida", "Llegada Est.", "Estado");
            foreach (var v in vuelos)
                SpectreHelper.AgregarFila(tabla,
                    v.Id.ToString(), v.NumeroVuelo.Valor,
                    v.FechaSalida.Valor.ToString("yyyy-MM-dd HH:mm"),
                    v.FechaLlegadaEstimada.Valor.ToString("yyyy-MM-dd HH:mm"),
                    v.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);

            // Mostrar tarifas activas para cada vuelo encontrado
            SpectreHelper.MostrarSubtitulo("Tarifas disponibles");
            foreach (var v in vuelos)
            {
                await using var scopeT = _provider.CreateAsyncScope();
                var tarifas = await scopeT.ServiceProvider
                    .GetRequiredService<GetActiveFaresByRouteUseCase>().ExecuteAsync(v.RutaId);
                if (tarifas.Count == 0) continue;
                SpectreHelper.MostrarInfo($"Vuelo {v.NumeroVuelo.Valor} (ID {v.Id}):");
                var tablaT = SpectreHelper.CrearTabla("TarifaID", "Nombre", "ClaseID", "Total", "Cambios", "Reembolso");
                foreach (var t in tarifas)
                    SpectreHelper.AgregarFila(tablaT,
                        t.Id.ToString(), t.Nombre.Valor,
                        t.ClaseServicioId.ToString(),
                        t.PrecioTotal.Valor.ToString("C2"),
                        t.PermiteCambios.Valor ? "Sí" : "No",
                        t.PermiteReembolso.Valor ? "Sí" : "No");
                SpectreHelper.MostrarTabla(tablaT);
            }

            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerAsientosAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetAvailableSeatsByFlightUseCase>().ExecuteAsync(vueloId);

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos disponibles para ese vuelo.");
                SpectreHelper.EsperarTecla(); return;
            }

            var tabla = SpectreHelper.CrearTabla("AsientoID", "Estado");
            foreach (var s in asientos)
                SpectreHelper.AgregarFila(tabla, s.AsientoId.ToString(), s.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearReservaAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Reserva");

        var vueloId   = SpectreHelper.PedirEntero("ID del vuelo");
        var tarifaId  = SpectreHelper.PedirEntero("ID de la tarifa");
        var valorTotal = SpectreHelper.PedirDecimal("Valor total");
        var obs        = SpectreHelper.PedirTexto("Observaciones (opcional)");
        string? obsOpc = string.IsNullOrWhiteSpace(obs) ? null : obs;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            // Obtener clienteId del usuario en sesión
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);

            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<CreateBookingUseCase>()
                .ExecuteAsync(clienteId, vueloId, tarifaId, valorTotal, obsOpc);
            SpectreHelper.MostrarExito(
                $"Reserva creada exitosamente.\n" +
                $"  Código : {b.CodigoReserva.Valor}\n" +
                $"  Estado : {b.Estado.Valor}\n" +
                $"  Expira : {b.FechaExpiracion.Valor:yyyy-MM-dd HH:mm}\n" +
                $"  Total  : {b.ValorTotal.Valor:C2}");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarPasajeroAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Pasajero");
        var reservaId  = SpectreHelper.PedirEntero("ID de la reserva");
        var personaId  = SpectreHelper.PedirEntero("ID de la persona (pasajero)");
        var tipoOpc    = SpectreHelper.SeleccionarOpcionTexto("Tipo de pasajero", ["ADULTO", "MENOR", "INFANTE"]);
        var asientoStr = SpectreHelper.PedirTexto("ID del asiento (opcional, Enter para omitir)");
        int? asientoId = int.TryParse(asientoStr, out var av) && av > 0 ? av : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<AddPassengerUseCase>()
                .ExecuteAsync(reservaId, personaId, tipoOpc, asientoId);
            SpectreHelper.MostrarExito($"Pasajero agregado (ID {p.Id}). Tipo: {p.TipoPasajero.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    // Resuelve el clienteId buscando por usuarioId en la BD
    private async Task<int> ObtenerClienteIdAsync(IServiceProvider sp)
    {
        var clientes = await sp.GetRequiredService<AirTicketSystem.modules.client.Application.UseCases.GetAllClientsUseCase>()
            .ExecuteAsync();
        var cliente = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente asociado a su usuario. Contacte al administrador.");
        return cliente.Id;
    }

    private async Task MostrarAeropuertosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor, a.CodigoIata.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
