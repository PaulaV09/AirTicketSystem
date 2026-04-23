// src/UI/Admin/Reservations/ReservationsMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class ReservationsMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public ReservationsMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Reservas y Pasajeros");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Reservas",
                    "Pasajeros de reserva",
                    "Check-in",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Reservas":
                    await new BookingMenu(_provider, _session).MostrarAsync();
                    break;
                case "Pasajeros de reserva":
                    await new PassengerMenu(_provider).MostrarAsync();
                    break;
                case "Check-in":
                    await new CheckInMenu(_provider).MostrarAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }
}
