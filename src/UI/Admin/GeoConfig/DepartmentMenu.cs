// src/UI/Admin/GeoConfig/DepartmentMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.department.Application.UseCases;
using AirTicketSystem.modules.region.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class DepartmentMenu
{
    private readonly IServiceProvider _provider;

    public DepartmentMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Departamentos / Provincias");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por región", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":      await ListarTodosAsync();     break;
                case "Listar por región": await ListarPorRegionAsync(); break;
                case "Crear":             await CrearAsync();           break;
                case "Editar":            await EditarAsync();          break;
                case "Eliminar":          await EliminarAsync();        break;
                case "Volver":            return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllDepartmentsUseCase>();
            var lista = await uc.ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay departamentos registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "RegiónID");
            foreach (var d in lista)
                SpectreHelper.AgregarFila(tabla, d.Id.ToString(), d.Nombre.Valor,
                    d.Codigo?.Valor ?? "-", d.RegionId.ToString());

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
            var uc    = scope.ServiceProvider.GetRequiredService<GetDepartmentsByRegionUseCase>();
            var lista = await uc.ExecuteAsync(regionId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código");
            foreach (var d in lista)
                SpectreHelper.AgregarFila(tabla, d.Id.ToString(), d.Nombre.Valor, d.Codigo?.Valor ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Departamento");

        await MostrarRegionesAsync();

        var regionId = SpectreHelper.PedirEntero("ID de la región");
        var nombre   = SpectreHelper.PedirTexto("Nombre");
        var codigo   = SpectreHelper.PedirTexto("Código (opcional, Enter para omitir)");
        string? codigoOpc = string.IsNullOrWhiteSpace(codigo) ? null : codigo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<CreateDepartmentUseCase>();
            var result = await uc.ExecuteAsync(regionId, nombre, codigoOpc);
            SpectreHelper.MostrarExito($"Departamento '{result.Nombre.Valor}' creado (ID {result.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Departamento");
        var id     = SpectreHelper.PedirEntero("ID del departamento");
        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
        var codigo = SpectreHelper.PedirTexto("Nuevo código (opcional, Enter para omitir)");
        string? codigoOpc = string.IsNullOrWhiteSpace(codigo) ? null : codigo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc     = scope.ServiceProvider.GetRequiredService<UpdateDepartmentUseCase>();
            var result = await uc.ExecuteAsync(id, nombre, codigoOpc);
            SpectreHelper.MostrarExito($"Departamento '{result.Nombre.Valor}' actualizado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar Departamento");
        var id = SpectreHelper.PedirEntero("ID del departamento a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        {
            SpectreHelper.MostrarInfo("Operación cancelada.");
            SpectreHelper.EsperarTecla();
            return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<DeleteDepartmentUseCase>();
            await uc.ExecuteAsync(id);
            SpectreHelper.MostrarExito("Departamento eliminado correctamente.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarRegionesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc    = scope.ServiceProvider.GetRequiredService<GetAllRegionsUseCase>();
            var lista = await uc.ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Región", "PaísID");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor, r.PaisId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
