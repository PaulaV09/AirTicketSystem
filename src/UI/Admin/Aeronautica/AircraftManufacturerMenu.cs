// src/UI/Admin/Aeronautica/AircraftManufacturerMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;
using AirTicketSystem.modules.country.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftManufacturerMenu
{
    private readonly IServiceProvider _provider;

    public AircraftManufacturerMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Fabricantes de Aeronaves");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por país", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":    await ListarTodosAsync();     break;
                case "Listar por país": await ListarPorPaisAsync();   break;
                case "Crear":           await CrearAsync();           break;
                case "Editar":          await EditarAsync();          break;
                case "Eliminar":        await EliminarAsync();        break;
                case "Volver":          return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftManufacturersUseCase>().ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Sitio Web", "PaísID");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla, f.Id.ToString(), f.Nombre.Valor,
                    f.SitioWeb?.Valor ?? "-", f.PaisId.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorPaisAsync()
    {
        var paisId = SpectreHelper.PedirEntero("ID del país");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAircraftManufacturersByCountryUseCase>().ExecuteAsync(paisId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Sitio Web");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla, f.Id.ToString(), f.Nombre.Valor, f.SitioWeb?.Valor ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Fabricante");

        await MostrarPaisesAsync();

        var paisId  = SpectreHelper.PedirEntero("ID del país de origen");
        var nombre  = SpectreHelper.PedirTexto("Nombre del fabricante");
        var web     = SpectreHelper.PedirTexto("Sitio web (opcional, Enter para omitir)");
        string? webOpc = string.IsNullOrWhiteSpace(web) ? null : web;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<CreateAircraftManufacturerUseCase>()
                .ExecuteAsync(paisId, nombre, webOpc);
            SpectreHelper.MostrarExito($"Fabricante '{r.Nombre.Valor}' creado (ID {r.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Fabricante");
        var id     = SpectreHelper.PedirEntero("ID del fabricante");
        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
        var web    = SpectreHelper.PedirTexto("Nuevo sitio web (opcional, Enter para omitir)");
        string? webOpc = string.IsNullOrWhiteSpace(web) ? null : web;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<UpdateAircraftManufacturerUseCase>()
                .ExecuteAsync(id, nombre, webOpc);
            SpectreHelper.MostrarExito($"Fabricante '{r.Nombre.Valor}' actualizado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar Fabricante");
        var id = SpectreHelper.PedirEntero("ID del fabricante a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        { SpectreHelper.MostrarInfo("Cancelado."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<DeleteAircraftManufacturerUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Fabricante eliminado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarPaisesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllCountriesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "País", "ISO-2");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla, p.Id.ToString(), p.Nombre.Valor, p.CodigoIso2.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
