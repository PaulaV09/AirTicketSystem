// src/modules/seatavailability/Domain/aggregate/SeatAvailability.cs
using AirTicketSystem.modules.seatavailability.Domain.ValueObjects;

namespace AirTicketSystem.modules.seatavailability.Domain.aggregate;

public sealed class SeatAvailability
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int AsientoId { get; private set; }
    public EstadoSeatAvailability Estado { get; private set; } = null!;

    private SeatAvailability() { }

    public static SeatAvailability Crear(int vueloId, int asientoId)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (asientoId <= 0)
            throw new ArgumentException("El asiento es obligatorio.");

        return new SeatAvailability
        {
            VueloId   = vueloId,
            AsientoId = asientoId,
            Estado    = EstadoSeatAvailability.Disponible()
        };
    }

    // ── Máquina de estados ───────────────────────────────────

    public void Reservar()
    {
        if (!Estado.PuedeReservarse)
            throw new InvalidOperationException(
                $"El asiento no puede reservarse en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Reservado();
    }

    public void Ocupar()
    {
        if (!Estado.PuedeTransicionarA(EstadoSeatAvailability.Ocupado()))
            throw new InvalidOperationException(
                $"El asiento no puede ocuparse en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Ocupado();
    }

    public void Liberar()
    {
        if (!Estado.PuedeTransicionarA(EstadoSeatAvailability.Disponible()))
            throw new InvalidOperationException(
                $"El asiento no puede liberarse en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Disponible();
    }

    public void Bloquear()
    {
        if (!Estado.PuedeTransicionarA(EstadoSeatAvailability.Bloqueado()))
            throw new InvalidOperationException(
                $"El asiento no puede bloquearse en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Bloqueado();
    }

    public void Desbloquear()
    {
        if (!Estado.PuedeTransicionarA(EstadoSeatAvailability.Disponible()))
            throw new InvalidOperationException(
                $"El asiento no puede desbloquearse en estado '{Estado}'.");

        Estado = EstadoSeatAvailability.Disponible();
    }

    // Propiedades de negocio
    public bool EstaDisponible => Estado.PuedeReservarse;
    public bool EstaOcupado    => Estado.EstaOcupado;

    public override string ToString() =>
        $"Vuelo #{VueloId} — Asiento #{AsientoId} [{Estado}]";
}