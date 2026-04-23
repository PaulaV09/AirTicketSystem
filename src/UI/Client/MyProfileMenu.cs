// src/UI/Client/MyProfileMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.user.Application.UseCases;

namespace AirTicketSystem.UI.Client;

public sealed class MyProfileMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public MyProfileMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Mi Perfil");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver mi información",
                    "Cambiar contraseña",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver mi información":  await VerInfoAsync();           break;
                case "Cambiar contraseña":  await CambiarPasswordAsync();   break;
                case "Volver":              return;
            }
        }
    }

    private async Task VerInfoAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var u = await scope.ServiceProvider.GetRequiredService<GetUserByIdUseCase>()
                .ExecuteAsync(_session.CurrentUserId);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",            u.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Username",      u.Username.Valor);
            SpectreHelper.AgregarFila(tabla, "Activo",        u.Activo.Valor ? "Sí" : "No");
            SpectreHelper.AgregarFila(tabla, "Registro",      u.FechaRegistro.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Último acceso", u.UltimoLogin?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CambiarPasswordAsync()
    {
        var actual   = SpectreHelper.PedirTexto("Contraseña actual");
        var nueva    = SpectreHelper.PedirTexto("Nueva contraseña");
        var confirma = SpectreHelper.PedirTexto("Confirme la nueva contraseña");

        if (nueva != confirma)
        {
            SpectreHelper.MostrarError("Las contraseñas no coinciden.");
            SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            // Verificar contraseña actual
            await using var scopeV = _provider.CreateAsyncScope();
            var u = await scopeV.ServiceProvider.GetRequiredService<GetUserByIdUseCase>()
                .ExecuteAsync(_session.CurrentUserId);

            if (!PasswordHasher.Verify(actual, u.PasswordHash.Valor))
                throw new InvalidOperationException("La contraseña actual es incorrecta.");

            var nuevoHash = PasswordHasher.Hash(nueva);
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ChangePasswordUseCase>()
                .ExecuteAsync(_session.CurrentUserId, nuevoHash);
            SpectreHelper.MostrarExito("Contraseña actualizada correctamente.");
        });
        SpectreHelper.EsperarTecla();
    }
}
