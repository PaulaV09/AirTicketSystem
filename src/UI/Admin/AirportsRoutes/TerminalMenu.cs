// src/UI/Admin/AirportsRoutes/TerminalMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.terminal.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class TerminalMenu
{
    private readonly IServiceProvider _provider;

    public TerminalMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Terminales");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar por aeropuerto", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar por aeropuerto": await ListarAsync();   break;
                case "Crear":                 await CrearAsync();    break;
                case "Editar":                await EditarAsync();   break;
                case "Eliminar":              await EliminarAsync(); break;
                case "Volver":                return;
            }
        }
    }

    private async Task ListarAsync()
    {
        await MostrarAeropuertosAsync();
        var aeropuertoId = SpectreHelper.PedirEntero("ID del aeropuerto");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetTerminalsByAirportUseCase>().ExecuteAsync(aeropuertoId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin terminales."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Descripción");
            foreach (var t in lista)
                SpectreHelper.AgregarFila(tabla, t.Id.ToString(), t.Nombre.Valor,
                    t.Descripcion?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Terminal");
        await MostrarAeropuertosAsync();

        var aeropuertoId = SpectreHelper.PedirEntero("ID del aeropuerto");
        var nombre       = SpectreHelper.PedirTexto("Nombre de la terminal (ej: Terminal 1)");
        var descripcion  = SpectreHelper.PedirTexto("Descripción (opcional)");
        string? descOpc  = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateTerminalUseCase>()
                .ExecuteAsync(aeropuertoId, nombre, descOpc);
            SpectreHelper.MostrarExito($"Terminal '{r.Nombre.Valor}' creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Terminal");
        var id          = SpectreHelper.PedirEntero("ID de la terminal");
        var nombre      = SpectreHelper.PedirTexto("Nuevo nombre");
        var descripcion = SpectreHelper.PedirTexto("Nueva descripción (opcional)");
        string? descOpc = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateTerminalUseCase>()
                .ExecuteAsync(id, nombre, descOpc);
            SpectreHelper.MostrarExito($"Terminal '{r.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la terminal a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteTerminalUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Terminal eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarAeropuertosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Aeropuerto", "IATA", "Activo");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.Activo.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
