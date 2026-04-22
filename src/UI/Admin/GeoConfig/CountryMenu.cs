// src/UI/Admin/GeoConfig/CountryMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.country.Application.UseCases;
using AirTicketSystem.modules.continent.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class CountryMenu
{
    private readonly IServiceProvider _provider;

    public CountryMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Países");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por continente", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":           await ListarTodosAsync();       break;
                case "Listar por continente":  await ListarPorContinenteAsync(); break;
                case "Crear":                  await CrearAsync();             break;
                case "Editar":                 await EditarAsync();            break;
                case "Eliminar":               await EliminarAsync();          break;
                case "Volver":                 return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<GetAllCountriesUseCase>();
            var lista = await uc.ExecuteAsync();

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay países registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "ISO-2", "ISO-3", "ContinenteID");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.CodigoIso2.Valor,
                    c.CodigoIso3.Valor, c.ContinenteId.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorContinenteAsync()
    {
        var continenteId = SpectreHelper.PedirEntero("ID del continente");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<GetCountriesByContinentUseCase>();
            var lista = await uc.ExecuteAsync(continenteId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "ISO-2", "ISO-3");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.CodigoIso2.Valor, c.CodigoIso3.Valor);

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo País");

        await MostrarContinentesAsync();

        var continenteId = SpectreHelper.PedirEntero("ID del continente");
        var nombre       = SpectreHelper.PedirTexto("Nombre");
        var iso2         = SpectreHelper.PedirTexto("Código ISO-2 (ej: CO)");
        var iso3         = SpectreHelper.PedirTexto("Código ISO-3 (ej: COL)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<CreateCountryUseCase>();
            var result = await uc.ExecuteAsync(continenteId, nombre, iso2, iso3);
            SpectreHelper.MostrarExito($"País '{result.Nombre.Valor}' creado (ID {result.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar País");
        var id     = SpectreHelper.PedirEntero("ID del país");
        var nombre = SpectreHelper.PedirTexto("Nuevo nombre");
        var iso2   = SpectreHelper.PedirTexto("Nuevo ISO-2");
        var iso3   = SpectreHelper.PedirTexto("Nuevo ISO-3");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<UpdateCountryUseCase>();
            var result = await uc.ExecuteAsync(id, nombre, iso2, iso3);
            SpectreHelper.MostrarExito($"País '{result.Nombre.Valor}' actualizado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Eliminar País");
        var id = SpectreHelper.PedirEntero("ID del país a eliminar");

        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?"))
        {
            SpectreHelper.MostrarInfo("Operación cancelada.");
            SpectreHelper.EsperarTecla();
            return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<DeleteCountryUseCase>();
            await uc.ExecuteAsync(id);
            SpectreHelper.MostrarExito("País eliminado correctamente.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarContinentesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var uc = scope.ServiceProvider.GetRequiredService<GetAllContinentsUseCase>();
            var lista = await uc.ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Continente");
            foreach (var c in lista) SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
