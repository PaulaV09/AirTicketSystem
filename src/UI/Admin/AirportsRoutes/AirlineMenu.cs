// src/UI/Admin/AirportsRoutes/AirlineMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.country.Application.UseCases;
using AirTicketSystem.modules.emailtype.Application.UseCases;
using AirTicketSystem.modules.phonetype.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class AirlineMenu
{
    private readonly IServiceProvider _provider;

    public AirlineMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aerolíneas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Listar activas",
                    "Crear",
                    "Editar",
                    "Activar",
                    "Desactivar",
                    "Agregar email",
                    "Eliminar email",
                    "Agregar teléfono",
                    "Eliminar teléfono",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":     await ListarTodasAsync();    break;
                case "Listar activas":   await ListarActivasAsync();  break;
                case "Crear":            await CrearAsync();          break;
                case "Editar":           await EditarAsync();         break;
                case "Activar":          await ActivarAsync();        break;
                case "Desactivar":       await DesactivarAsync();     break;
                case "Agregar email":    await AgregarEmailAsync();   break;
                case "Eliminar email":   await EliminarEmailAsync();  break;
                case "Agregar teléfono": await AgregarTelefonoAsync(); break;
                case "Eliminar teléfono": await EliminarTelefonoAsync(); break;
                case "Volver":           return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin aerolíneas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Comercial", "IATA", "ICAO", "Activa");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.NombreComercial?.Valor ?? "-",
                    a.CodigoIata.Valor, a.CodigoIcao.Valor,
                    a.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirlinesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay aerolíneas activas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.CodigoIcao.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Aerolínea");
        await MostrarPaisesAsync();

        var paisId         = SpectreHelper.PedirEntero("ID del país");
        var iata           = SpectreHelper.PedirTexto("Código IATA (2 letras, ej: AV)");
        var icao           = SpectreHelper.PedirTexto("Código ICAO (3 letras, ej: AVA)");
        var nombre         = SpectreHelper.PedirTexto("Nombre oficial");
        var nombreComercial = SpectreHelper.PedirTexto("Nombre comercial (opcional)");
        var sitioWeb       = SpectreHelper.PedirTexto("Sitio web (opcional)");
        string? ncOpc  = string.IsNullOrWhiteSpace(nombreComercial) ? null : nombreComercial;
        string? webOpc = string.IsNullOrWhiteSpace(sitioWeb)        ? null : sitioWeb;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAirlineUseCase>()
                .ExecuteAsync(paisId, iata, icao, nombre, ncOpc, webOpc);
            SpectreHelper.MostrarExito($"Aerolínea '{r.Nombre.Valor}' [{r.CodigoIata.Valor}] creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Aerolínea");
        var id             = SpectreHelper.PedirEntero("ID de la aerolínea");
        var nombre         = SpectreHelper.PedirTexto("Nuevo nombre oficial");
        var nombreComercial = SpectreHelper.PedirTexto("Nuevo nombre comercial (opcional)");
        var sitioWeb       = SpectreHelper.PedirTexto("Nuevo sitio web (opcional)");
        string? ncOpc  = string.IsNullOrWhiteSpace(nombreComercial) ? null : nombreComercial;
        string? webOpc = string.IsNullOrWhiteSpace(sitioWeb)        ? null : sitioWeb;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAirlineUseCase>()
                .ExecuteAsync(id, nombre, ncOpc, webOpc);
            SpectreHelper.MostrarExito($"Aerolínea '{r.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la aerolínea a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateAirlineUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aerolínea activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la aerolínea a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateAirlineUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Aerolínea desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarEmailAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Email a Aerolínea");
        await ListarActivasAsync();
        var aerolineaId = SpectreHelper.PedirEntero("ID de la aerolínea");
        await MostrarTiposEmailAsync();
        var tipoEmailId = SpectreHelper.PedirEntero("ID del tipo de email");
        var email       = SpectreHelper.PedirTexto("Dirección de email");
        var esPrincipal = SpectreHelper.Confirmar("¿Es el email principal?");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<AddAirlineEmailUseCase>()
                .ExecuteAsync(aerolineaId, tipoEmailId, email, esPrincipal);
            SpectreHelper.MostrarExito($"Email '{r.Email.Valor}' agregado (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarEmailAsync()
    {
        var emailId = SpectreHelper.PedirEntero("ID del email a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveAirlineEmailUseCase>()
                .ExecuteAsync(emailId);
            SpectreHelper.MostrarExito("Email eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarTelefonoAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Teléfono a Aerolínea");
        await ListarActivasAsync();
        var aerolineaId  = SpectreHelper.PedirEntero("ID de la aerolínea");
        await MostrarTiposTelefonoAsync();
        var tipoTelId    = SpectreHelper.PedirEntero("ID del tipo de teléfono");
        var numero       = SpectreHelper.PedirTexto("Número de teléfono");
        var indicativo   = SpectreHelper.PedirTexto("Indicativo país (opcional, ej: +57)");
        var esPrincipal  = SpectreHelper.Confirmar("¿Es el teléfono principal?");
        string? indOpc   = string.IsNullOrWhiteSpace(indicativo) ? null : indicativo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<AddAirlinePhoneUseCase>()
                .ExecuteAsync(aerolineaId, tipoTelId, numero, indOpc, esPrincipal);
            SpectreHelper.MostrarExito($"Teléfono '{r.Numero.Valor}' agregado (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarTelefonoAsync()
    {
        var telefonoId = SpectreHelper.PedirEntero("ID del teléfono a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveAirlinePhoneUseCase>()
                .ExecuteAsync(telefonoId);
            SpectreHelper.MostrarExito("Teléfono eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarPaisesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllCountriesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "País", "ISO-2");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla, p.Id.ToString(), p.Nombre.Valor, p.CodigoIso2.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarTiposEmailAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllEmailTypesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Tipo");
            foreach (var t in lista)
                SpectreHelper.AgregarFila(tabla, t.Id.ToString(), t.Descripcion.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarTiposTelefonoAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllPhoneTypesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Tipo");
            foreach (var t in lista)
                SpectreHelper.AgregarFila(tabla, t.Id.ToString(), t.Descripcion.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
