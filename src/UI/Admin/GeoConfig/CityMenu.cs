// src/UI/Admin/GeoConfig/CityMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.city.Application.UseCases;
using AirTicketSystem.modules.department.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class CityMenu
{
    private readonly IServiceProvider _provider;

    public CityMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Ciudades");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todas", "Listar por región", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todas":      await ListarTodasAsync();     break;
                case "Listar por región": await ListarPorRegionAsync(); break;
                case "Crear":             await CrearAsync();           break;
                case "Editar":            await EditarAsync();          break;
                case "Eliminar":          await EliminarAsync();        break;
                case "Volver":            return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllCitiesUseCase>();
            var lista = await uc.ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay ciudades registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Cód. Postal", "DeptoID");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor,
                    c.CodigoPostal?.Valor ?? "-", c.DepartamentoId.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorRegionAsync()
    {
        var regionId = SpectreHelper.PedirEntero("ID de la región");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetCitiesByDepartmentUseCase>();
            var lista = await uc.ExecuteAsync(regionId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Cód. Postal");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.CodigoPostal?.Valor ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Ciudad");

        await MostrarDepartamentosAsync();

        var dptoId = SpectreHelper.PedirEntero("ID del departamento");
        var nombre = SpectreHelper.PedirTexto("Nombre");
        var cp     = SpectreHelper.PedirTexto("Código postal (opcional, Enter para omitir)");
        string? cpOpc = string.IsNullOrWhiteSpace(cp) ? null : cp;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<CreateCityUseCase>();
            var result = await uc.ExecuteAsync(dptoId, nombre, cpOpc);
            SpectreHelper.MostrarExito($"Ciudad '{result.Nombre.Valor}' creada (ID {result.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Ciudad");
        var id     = SpectreHelper.PedirEntero("ID de la ciudad");
        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
        var cp     = SpectreHelper.PedirTexto("Nuevo código postal (opcional, Enter para omitir)");
        string? cpOpc = string.IsNullOrWhiteSpace(cp) ? null : cp;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<UpdateCityUseCase>();
            var result = await uc.ExecuteAsync(id, nombre, cpOpc);
            SpectreHelper.MostrarExito($"Ciudad '{result.Nombre.Valor}' actualizada.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar Ciudad");
        var id = SpectreHelper.PedirEntero("ID de la ciudad a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        {
            SpectreHelper.MostrarInfo("Operación cancelada.");
            SpectreHelper.EsperarTecla();
            return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<DeleteCityUseCase>();
            await uc.ExecuteAsync(id);
            SpectreHelper.MostrarExito("Ciudad eliminada correctamente.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarDepartamentosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllDepartmentsUseCase>();
            var lista = await uc.ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Departamento", "RegiónID");
            foreach (var d in lista)
                SpectreHelper.AgregarFila(tabla, d.Id.ToString(), d.Nombre.Valor, d.RegionId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
