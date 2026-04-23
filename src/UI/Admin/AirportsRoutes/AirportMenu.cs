// src/UI/Admin/AirportsRoutes/AirportMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.city.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class AirportMenu
{
    private readonly IServiceProvider _provider;

    public AirportMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aeropuertos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar activos", "Crear", "Editar", "Activar", "Desactivar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":  await ListarTodosAsync();  break;
                case "Listar activos": await ListarActivosAsync(); break;
                case "Crear":         await CrearAsync();         break;
                case "Editar":        await EditarAsync();        break;
                case "Activar":       await ActivarAsync();       break;
                case "Desactivar":    await DesactivarAsync();    break;
                case "Volver":        return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO", "Activo", "CiudadID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.CodigoIcao.Valor,
                    a.Activo.Valor ? "Sí" : "No", a.CiudadId.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay aeropuertos activos."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO", "CiudadID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.CodigoIcao.Valor, a.CiudadId.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Aeropuerto");
        await MostrarCiudadesAsync();

        var ciudadId  = SpectreHelper.PedirEntero("ID de la ciudad");
        var iata      = SpectreHelper.PedirTexto("Código IATA (3 letras, ej: BOG)");
        var icao      = SpectreHelper.PedirTexto("Código ICAO (4 letras, ej: SKBO)");
        var nombre    = SpectreHelper.PedirTexto("Nombre del aeropuerto");
        var direccion = SpectreHelper.PedirTexto("Dirección (opcional, Enter para omitir)");
        string? dirOpc = string.IsNullOrWhiteSpace(direccion) ? null : direccion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAirportUseCase>()
                .ExecuteAsync(ciudadId, iata, icao, nombre, dirOpc);
            SpectreHelper.MostrarExito($"Aeropuerto '{r.Nombre.Valor}' [{r.CodigoIata.Valor}] creado (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Aeropuerto");
        var id        = SpectreHelper.PedirEntero("ID del aeropuerto");
        var nombre    = SpectreHelper.PedirTexto("Nuevo nombre");
        var direccion = SpectreHelper.PedirTexto("Nueva dirección (opcional, Enter para omitir)");
        string? dirOpc = string.IsNullOrWhiteSpace(direccion) ? null : direccion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAirportUseCase>()
                .ExecuteAsync(id, nombre, dirOpc);
            SpectreHelper.MostrarExito($"Aeropuerto '{r.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del aeropuerto a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateAirportUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aeropuerto activado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del aeropuerto a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateAirportUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aeropuerto desactivado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarCiudadesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllCitiesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Ciudad", "DeptoID");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.DepartamentoId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
