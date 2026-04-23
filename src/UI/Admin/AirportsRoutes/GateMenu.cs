// src/UI/Admin/AirportsRoutes/GateMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.gate.Application.UseCases;
using AirTicketSystem.modules.terminal.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class GateMenu
{
    private readonly IServiceProvider _provider;

    public GateMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Puertas de Embarque");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar por terminal", "Listar activas por terminal", "Crear", "Editar", "Activar", "Desactivar", "Volver"]);

            switch (opcion)
            {
                case "Listar por terminal":         await ListarPorTerminalAsync();       break;
                case "Listar activas por terminal": await ListarActivasPorTerminalAsync(); break;
                case "Crear":                       await CrearAsync();                   break;
                case "Editar":                      await EditarAsync();                  break;
                case "Activar":                     await ActivarAsync();                 break;
                case "Desactivar":                  await DesactivarAsync();              break;
                case "Volver":                      return;
            }
        }
    }

    private async Task ListarPorTerminalAsync()
    {
        var terminalId = SpectreHelper.PedirEntero("ID de la terminal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetGatesByTerminalUseCase>().ExecuteAsync(terminalId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin puertas."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Código", "Activa");
            foreach (var g in lista)
                SpectreHelper.AgregarFila(tabla, g.Id.ToString(), g.Codigo.Valor,
                    g.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasPorTerminalAsync()
    {
        var terminalId = SpectreHelper.PedirEntero("ID de la terminal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetActiveGatesByTerminalUseCase>().ExecuteAsync(terminalId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin puertas activas."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Código");
            foreach (var g in lista)
                SpectreHelper.AgregarFila(tabla, g.Id.ToString(), g.Codigo.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Puerta de Embarque");
        await MostrarTerminalesAsync();

        var terminalId = SpectreHelper.PedirEntero("ID de la terminal");
        var codigo     = SpectreHelper.PedirTexto("Código de la puerta (ej: A1, B23)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateGateUseCase>()
                .ExecuteAsync(terminalId, codigo);
            SpectreHelper.MostrarExito($"Puerta '{r.Codigo.Valor}' creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Puerta");
        var id     = SpectreHelper.PedirEntero("ID de la puerta");
        var codigo = SpectreHelper.PedirTexto("Nuevo código");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateGateUseCase>()
                .ExecuteAsync(id, codigo);
            SpectreHelper.MostrarExito($"Puerta '{r.Codigo.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la puerta a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateGateUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Puerta activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la puerta a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateGateUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Puerta desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarTerminalesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            // No existe GetAllTerminalsUseCase, así que pedimos ID directo
            SpectreHelper.MostrarInfo("Ingrese el ID de la terminal (consulte la sección Terminales para ver el listado).");
        });
    }
}
