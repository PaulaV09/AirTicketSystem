// src/UI/Admin/Aeronautica/AircraftModelMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftModelMenu
{
    private readonly IServiceProvider _provider;

    public AircraftModelMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Modelos de Aeronave");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por fabricante", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":          await ListarTodosAsync();         break;
                case "Listar por fabricante": await ListarPorFabricanteAsync(); break;
                case "Crear":                 await CrearAsync();               break;
                case "Editar":                await EditarAsync();              break;
                case "Eliminar":              await EliminarAsync();            break;
                case "Volver":                return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftModelsUseCase>().ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "FabricanteID", "Autonomía(km)", "Velocidad(km/h)");
            foreach (var m in lista)
                SpectreHelper.AgregarFila(tabla,
                    m.Id.ToString(), m.Nombre.Valor, m.CodigoModelo.Valor,
                    m.FabricanteId.ToString(),
                    m.AutonomiKm?.Valor.ToString() ?? "-",
                    m.VelocidadCruceroKmh?.Valor.ToString() ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorFabricanteAsync()
    {
        await MostrarFabricantesAsync();
        var fabricanteId = SpectreHelper.PedirEntero("ID del fabricante");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAircraftModelsByManufacturerUseCase>().ExecuteAsync(fabricanteId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "Autonomía(km)");
            foreach (var m in lista)
                SpectreHelper.AgregarFila(tabla, m.Id.ToString(), m.Nombre.Valor,
                    m.CodigoModelo.Valor, m.AutonomiKm?.Valor.ToString() ?? "-");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Modelo de Aeronave");

        await MostrarFabricantesAsync();

        var fabricanteId = SpectreHelper.PedirEntero("ID del fabricante");
        var nombre       = SpectreHelper.PedirTexto("Nombre del modelo (ej: Boeing 737)");
        var codigo       = SpectreHelper.PedirTexto("Código del modelo (ej: B737)");
        var autonomiaStr = SpectreHelper.PedirTexto("Autonomía en km (opcional, Enter para omitir)");
        var velocidadStr = SpectreHelper.PedirTexto("Velocidad crucero km/h (opcional, Enter para omitir)");
        var descripcion  = SpectreHelper.PedirTexto("Descripción (opcional, Enter para omitir)");

        int? autonomia  = int.TryParse(autonomiaStr,  out var a) && a > 0 ? a : null;
        int? velocidad  = int.TryParse(velocidadStr,  out var v) && v > 0 ? v : null;
        string? descOpc = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<CreateAircraftModelUseCase>()
                .ExecuteAsync(fabricanteId, nombre, codigo, autonomia, velocidad, descOpc);
            SpectreHelper.MostrarExito($"Modelo '{r.Nombre.Valor}' creado (ID {r.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Modelo");
        var id           = SpectreHelper.PedirEntero("ID del modelo");
        var nombre       = SpectreHelper.PedirTexto("Nuevo nombre");
        var autonomiaStr = SpectreHelper.PedirTexto("Nueva autonomía km (opcional, Enter para omitir)");
        var velocidadStr = SpectreHelper.PedirTexto("Nueva velocidad km/h (opcional, Enter para omitir)");
        var descripcion  = SpectreHelper.PedirTexto("Nueva descripción (opcional, Enter para omitir)");

        int? autonomia  = int.TryParse(autonomiaStr,  out var a) && a > 0 ? a : null;
        int? velocidad  = int.TryParse(velocidadStr,  out var v) && v > 0 ? v : null;
        string? descOpc = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<UpdateAircraftModelUseCase>()
                .ExecuteAsync(id, nombre, autonomia, velocidad, descOpc);
            SpectreHelper.MostrarExito($"Modelo '{r.Nombre.Valor}' actualizado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar Modelo");
        var id = SpectreHelper.PedirEntero("ID del modelo a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        { SpectreHelper.MostrarInfo("Cancelado."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<DeleteAircraftModelUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Modelo eliminado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarFabricantesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftManufacturersUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Fabricante");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla, f.Id.ToString(), f.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
