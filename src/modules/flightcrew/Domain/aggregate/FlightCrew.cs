// src/modules/flightcrew/Domain/aggregate/FlightCrew.cs
using AirTicketSystem.modules.flightcrew.Domain.ValueObjects;

namespace AirTicketSystem.modules.flightcrew.Domain.aggregate;

public sealed class FlightCrew
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int TrabajadorId { get; private set; }
    public RolEnVueloFlightCrew RolEnVuelo { get; private set; } = null!;

    private FlightCrew() { }

    public static FlightCrew Crear(
        int vueloId,
        int trabajadorId,
        string rolEnVuelo)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (trabajadorId <= 0)
            throw new ArgumentException("El trabajador es obligatorio.");

        return new FlightCrew
        {
            VueloId     = vueloId,
            TrabajadorId = trabajadorId,
            RolEnVuelo  = RolEnVueloFlightCrew.Crear(rolEnVuelo)
        };
    }

    // Métodos de fábrica expresivos por rol
    public static FlightCrew AsignarPiloto(int vueloId, int trabajadorId)
        => Crear(vueloId, trabajadorId, "PILOTO");

    public static FlightCrew AsignarCopiloto(int vueloId, int trabajadorId)
        => Crear(vueloId, trabajadorId, "COPILOTO");

    public static FlightCrew AsignarSobrecargo(int vueloId, int trabajadorId)
        => Crear(vueloId, trabajadorId, "SOBRECARGO");

    public static FlightCrew AsignarAuxiliarVuelo(int vueloId, int trabajadorId)
        => Crear(vueloId, trabajadorId, "AUXILIAR_VUELO");

    public static FlightCrew AsignarAuxiliarSeguridad(int vueloId, int trabajadorId)
        => Crear(vueloId, trabajadorId, "AUXILIAR_SEGURIDAD");

    // Propiedades de negocio
    public bool EsParteDeCabina =>
        RolEnVuelo.EsParteDeCabina;

    public bool EsObligatorioParaDespegar =>
        RolEnVuelo.EsObligatorioParaDespegar;

    public override string ToString() =>
        $"Vuelo #{VueloId} — Trabajador #{TrabajadorId} [{RolEnVuelo}]";
}