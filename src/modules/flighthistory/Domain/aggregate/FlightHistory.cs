// src/modules/flighthistory/Domain/aggregate/FlightHistory.cs
using AirTicketSystem.modules.flighthistory.Domain.ValueObjects;

namespace AirTicketSystem.modules.flighthistory.Domain.aggregate;

public sealed class FlightHistory
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int? UsuarioId { get; private set; }
    public EstadoAnteriorFlightHistory EstadoAnterior { get; private set; } = null!;
    public EstadoNuevoFlightHistory EstadoNuevo { get; private set; } = null!;
    public FechaCambioFlightHistory FechaCambio { get; private set; } = null!;
    public MotivoFlightHistory? Motivo { get; private set; }

    private FlightHistory() { }

    public static FlightHistory Crear(
        int vueloId,
        string estadoAnterior,
        string estadoNuevo,
        int? usuarioId = null,
        string? motivo = null)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (estadoAnterior == estadoNuevo)
            throw new InvalidOperationException(
                "El estado anterior y el nuevo no pueden ser iguales.");

        return new FlightHistory
        {
            VueloId       = vueloId,
            UsuarioId     = usuarioId,
            EstadoAnterior = EstadoAnteriorFlightHistory.Crear(estadoAnterior),
            EstadoNuevo   = EstadoNuevoFlightHistory.Crear(estadoNuevo),
            FechaCambio   = FechaCambioFlightHistory.Ahora(),
            Motivo        = motivo is not null
                ? MotivoFlightHistory.Crear(motivo)
                : null
        };
    }

    // Propiedades de negocio
    public bool FueCancelacion =>
        EstadoNuevo.Valor == "CANCELADO";

    public bool FueDemora =>
        EstadoNuevo.Valor == "DEMORADO";

    public bool FueDesvio =>
        EstadoNuevo.Valor == "DESVIADO";

    public bool TieneMotivo => Motivo is not null;

    public override string ToString() =>
        $"Vuelo #{VueloId} | {EstadoAnterior} → {EstadoNuevo} " +
        $"[{FechaCambio}]";
}