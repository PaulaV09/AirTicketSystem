// src/UI/Admin/ClientesUsuarios/PersonasMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.person.Application.UseCases;

namespace AirTicketSystem.UI.Admin.ClientesUsuarios;

public sealed class PersonasMenu
{
    private readonly IServiceProvider _provider;

    public PersonasMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Personas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Buscar por ID",
                    "Buscar por documento",
                    "Crear persona",
                    "Actualizar persona",
                    "Agregar teléfono",
                    "Eliminar teléfono",
                    "Marcar teléfono principal",
                    "Agregar email",
                    "Eliminar email",
                    "Marcar email principal",
                    "Agregar dirección",
                    "Eliminar dirección",
                    "Marcar dirección principal",
                    "Eliminar persona",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":              await ListarTodasAsync();           break;
                case "Buscar por ID":             await BuscarPorIdAsync();           break;
                case "Buscar por documento":      await BuscarPorDocumentoAsync();    break;
                case "Crear persona":             await CrearAsync();                 break;
                case "Actualizar persona":        await ActualizarAsync();            break;
                case "Agregar teléfono":          await AgregarTelefonoAsync();       break;
                case "Eliminar teléfono":         await EliminarTelefonoAsync();      break;
                case "Marcar teléfono principal": await MarcarTelefonoPrincipalAsync(); break;
                case "Agregar email":             await AgregarEmailAsync();          break;
                case "Eliminar email":            await EliminarEmailAsync();         break;
                case "Marcar email principal":    await MarcarEmailPrincipalAsync();  break;
                case "Agregar dirección":         await AgregarDireccionAsync();      break;
                case "Eliminar dirección":        await EliminarDireccionAsync();     break;
                case "Marcar dirección principal": await MarcarDireccionPrincipalAsync(); break;
                case "Eliminar persona":          await EliminarAsync();              break;
                case "Volver":                    return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllPersonsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin personas registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "TipoDocID", "Número Doc", "Nombres", "Apellidos");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.TipoDocId.ToString(), p.NumeroDoc.Valor,
                    p.Nombres.Valor, p.Apellidos.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la persona");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<GetPersonByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",              p.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "TipoDocID",       p.TipoDocId.ToString());
            SpectreHelper.AgregarFila(tabla, "Número Doc",      p.NumeroDoc.Valor);
            SpectreHelper.AgregarFila(tabla, "Nombres",         p.Nombres.Valor);
            SpectreHelper.AgregarFila(tabla, "Apellidos",       p.Apellidos.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha nac.",      p.FechaNacimiento?.Valor.ToString("yyyy-MM-dd") ?? "-");
            SpectreHelper.AgregarFila(tabla, "GeneroID",        p.GeneroId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "NacionalidadID",  p.NacionalidadId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Menor de edad",   p.EsMenorDeEdad ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorDocumentoAsync()
    {
        var tipoDocId = SpectreHelper.PedirEntero("ID del tipo de documento");
        var numero    = SpectreHelper.PedirTexto("Número de documento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<GetPersonByDocumentUseCase>().ExecuteAsync(tipoDocId, numero);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",       p.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Nombres",  p.Nombres.Valor);
            SpectreHelper.AgregarFila(tabla, "Apellidos", p.Apellidos.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Persona");
        var tipoDocId  = SpectreHelper.PedirEntero("ID del tipo de documento");
        var numeroDoc  = SpectreHelper.PedirTexto("Número de documento");
        var nombres    = SpectreHelper.PedirTexto("Nombres");
        var apellidos  = SpectreHelper.PedirTexto("Apellidos");
        var fechaStr   = SpectreHelper.PedirTexto("Fecha de nacimiento (yyyy-MM-dd, opcional)");
        var generoStr  = SpectreHelper.PedirTexto("ID de género (opcional)");
        var nacionStr  = SpectreHelper.PedirTexto("ID de nacionalidad/país (opcional)");

        DateOnly? fecha   = DateOnly.TryParse(fechaStr, out var f) ? f : null;
        int?      genero  = int.TryParse(generoStr, out var g) && g > 0 ? g : null;
        int?      nacion  = int.TryParse(nacionStr, out var n) && n > 0 ? n : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<CreatePersonUseCase>()
                .ExecuteAsync(tipoDocId, numeroDoc, nombres, apellidos, fecha, genero, nacion);
            SpectreHelper.MostrarExito($"Persona '{p.NombreCompleto}' creada (ID {p.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarAsync()
    {
        var id        = SpectreHelper.PedirEntero("ID de la persona");
        var nombres   = SpectreHelper.PedirTexto("Nuevos nombres");
        var apellidos = SpectreHelper.PedirTexto("Nuevos apellidos");
        var fechaStr  = SpectreHelper.PedirTexto("Nueva fecha nac. (yyyy-MM-dd, opcional)");
        var generoStr = SpectreHelper.PedirTexto("Nuevo ID de género (opcional)");

        DateOnly? fecha  = DateOnly.TryParse(fechaStr, out var f) ? f : null;
        int?      genero = int.TryParse(generoStr, out var g) && g > 0 ? g : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<UpdatePersonUseCase>()
                .ExecuteAsync(id, nombres, apellidos, fecha, genero, null);
            SpectreHelper.MostrarExito("Persona actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarTelefonoAsync()
    {
        var personaId   = SpectreHelper.PedirEntero("ID de la persona");
        var tipoTelId   = SpectreHelper.PedirEntero("ID del tipo de teléfono");
        var numero      = SpectreHelper.PedirTexto("Número");
        var indicativo  = SpectreHelper.PedirTexto("Indicativo país (ej: +57, opcional)");
        var principalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal = principalStr.Trim().ToLower() == "s";
        string? indOpc  = string.IsNullOrWhiteSpace(indicativo) ? null : indicativo;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var ph = await scope.ServiceProvider.GetRequiredService<AddPersonPhoneUseCase>()
                .ExecuteAsync(personaId, tipoTelId, numero, indOpc, esPrincipal);
            SpectreHelper.MostrarExito($"Teléfono '{ph.Numero.Valor}' agregado (ID {ph.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarTelefonoAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del teléfono a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonPhoneUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Teléfono eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarTelefonoPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del teléfono a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonPhoneUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Teléfono marcado como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarEmailAsync()
    {
        var personaId    = SpectreHelper.PedirEntero("ID de la persona");
        var tipoEmailId  = SpectreHelper.PedirEntero("ID del tipo de email");
        var email        = SpectreHelper.PedirTexto("Dirección de email");
        var principalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal = principalStr.Trim().ToLower() == "s";

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var em = await scope.ServiceProvider.GetRequiredService<AddPersonEmailUseCase>()
                .ExecuteAsync(personaId, tipoEmailId, email, esPrincipal);
            SpectreHelper.MostrarExito($"Email '{em.Email.Valor}' agregado (ID {em.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarEmailAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del email a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonEmailUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Email eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarEmailPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del email a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonEmailUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Email marcado como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarDireccionAsync()
    {
        var personaId    = SpectreHelper.PedirEntero("ID de la persona");
        var tipoDirId    = SpectreHelper.PedirEntero("ID del tipo de dirección");
        var direccion    = SpectreHelper.PedirTexto("Dirección completa");
        var ciudadId     = SpectreHelper.PedirEntero("ID de la ciudad");
        var principalStr = SpectreHelper.PedirTexto("¿Es principal? (s/n)");
        bool esPrincipal = principalStr.Trim().ToLower() == "s";

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var addr = await scope.ServiceProvider.GetRequiredService<AddPersonAddressUseCase>()
                .ExecuteAsync(personaId, tipoDirId, ciudadId, direccion, null, null, esPrincipal);
            SpectreHelper.MostrarExito($"Dirección agregada (ID {addr.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarDireccionAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la dirección a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma eliminar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonAddressUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Dirección eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MarcarDireccionPrincipalAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la dirección a marcar como principal");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<SetPrincipalPersonAddressUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Dirección marcada como principal.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la persona a eliminar");
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeletePersonUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Persona eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
