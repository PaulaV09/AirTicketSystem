// src/UI/Admin/GeoConfig/RegionMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.region.Application.UseCases;
using AirTicketSystem.modules.country.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class RegionMenu
{
    private readonly IServiceProvider _provider;

    public RegionMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Regiones / Estados / Departamentos de País");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todas", "Listar por país", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todas":    await ListarTodasAsync();     break;
                case "Listar por país": await ListarPorPaisAsync();   break;
                case "Crear":           await CrearAsync();           break;
                case "Editar":          await EditarAsync();          break;
                case "Eliminar":        await EliminarAsync();        break;
                case "Volver":          return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllRegionsUseCase>();
            var lista = await uc.ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay regiones registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "PaísID");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor,
                    r.Codigo?.Valor ?? "-", r.PaisId.ToString());

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
            var uc    = scope.ServiceProvider.GetRequiredService<GetRegionsByCountryUseCase>();
            var lista = await uc.ExecuteAsync(paisId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor, r.Codigo?.Valor ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Región");

        await MostrarPaisesAsync();

        var paisId = SpectreHelper.PedirEntero("ID del país");
        var nombre = SpectreHelper.PedirTexto("Nombre");
        var codigo = SpectreHelper.PedirTexto("Código (opcional, Enter para omitir)");
        string? codigoOpc = string.IsNullOrWhiteSpace(codigo) ? null : codigo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<CreateRegionUseCase>();
            var result = await uc.ExecuteAsync(paisId, nombre, codigoOpc);
            SpectreHelper.MostrarExito($"Región '{result.Nombre.Valor}' creada (ID {result.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Región");
        var id     = SpectreHelper.PedirEntero("ID de la región");
        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
        var codigo = SpectreHelper.PedirTexto("Nuevo código (opcional, Enter para omitir)");
        string? codigoOpc = string.IsNullOrWhiteSpace(codigo) ? null : codigo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<UpdateRegionUseCase>();
            var result = await uc.ExecuteAsync(id, nombre, codigoOpc);
            SpectreHelper.MostrarExito($"Región '{result.Nombre.Valor}' actualizada.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar Región");
        var id = SpectreHelper.PedirEntero("ID de la región a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        {
            SpectreHelper.MostrarInfo("Operación cancelada.");
            SpectreHelper.EsperarTecla();
            return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<DeleteRegionUseCase>();
            await uc.ExecuteAsync(id);
            SpectreHelper.MostrarExito("Región eliminada correctamente.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarPaisesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllCountriesUseCase>();
            var lista = await uc.ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "País", "ISO-2");
            foreach (var p in lista) SpectreHelper.AgregarFila(tabla, p.Id.ToString(), p.Nombre.Valor, p.CodigoIso2.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
