// src/UI/Admin/AdminPortal.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin.GeoConfig;
using AirTicketSystem.UI.Admin.Aeronautica;

namespace AirTicketSystem.UI.Admin;

public sealed class AdminPortal
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public AdminPortal(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo(
                $"Portal Admin  —  {_session.CurrentUserName}");

            var opcion = SpectreHelper.SeleccionarOpcionTexto(
                "Seleccione un módulo",
                [
                    "1. Configuración geográfica",
                    "2. Gestión aeronáutica",
                    "3. Aeropuertos y rutas",
                    "4. Tripulación y personal",
                    "5. Vuelos",
                    "6. Reservas y pasajeros",
                    "7. Pagos y facturación",
                    "8. Usuarios y roles",
                    "Cerrar sesión"
                ]);

            if (opcion == "Cerrar sesión")
            {
                _session.Logout();
                return;
            }

            switch (opcion)
            {
                case "1. Configuración geográfica":
                    await new GeoConfigMenu(_provider).MostrarAsync();
                    break;

                case "2. Gestión aeronáutica":
                    await new AeronauticaMenu(_provider).MostrarAsync();
                    break;

                default:
                    SpectreHelper.MostrarTitulo(opcion);
                    SpectreHelper.MostrarAdvertencia("Módulo en construcción.");
                    SpectreHelper.EsperarTecla();
                    break;
            }
        }
    }
}
