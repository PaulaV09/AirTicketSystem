// src/modules/milesmovimiento/Domain/aggregate/MilesMovimiento.cs
using AirTicketSystem.modules.milesmovimiento.Domain.ValueObjects;

namespace AirTicketSystem.modules.milesmovimiento.Domain.aggregate;

// Un movimiento es un registro contable INMUTABLE: nunca se modifica,
// solo se crea. Sirve como auditoría completa del historial de millas.
public sealed class MilesMovimiento
{
    public int Id { get; private set; }
    public int CuentaId { get; private set; }
    public int? ReservaId { get; private set; }        // null en ajustes manuales
    public TipoMilesMovimiento Tipo { get; private set; } = null!;
    public CantidadMilesMovimiento Millas { get; private set; } = null!;
    public FechaMilesMovimiento Fecha { get; private set; } = null!;
    public DescripcionMilesMovimiento Descripcion { get; private set; } = null!;

    private MilesMovimiento() { }

    // ── Fábricas expresivas ──────────────────────────────────

    // Acumulación automática al completar un vuelo
    public static MilesMovimiento PorVueloCompletado(
        int cuentaId,
        int reservaId,
        int millas,
        string numeroVuelo)
    {
        if (cuentaId <= 0)
            throw new ArgumentException("La cuenta de millas es obligatoria.");

        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        return new MilesMovimiento
        {
            CuentaId    = cuentaId,
            ReservaId   = reservaId,
            Tipo        = TipoMilesMovimiento.Acumulacion(),
            Millas      = CantidadMilesMovimiento.Crear(millas),
            Fecha       = FechaMilesMovimiento.Ahora(),
            Descripcion = DescripcionMilesMovimiento.Crear(
                $"Acumulación por vuelo completado {numeroVuelo} — Reserva #{reservaId}")
        };
    }

    // Redención al realizar un pago parcial o total con millas
    public static MilesMovimiento PorRedencionEnReserva(
        int cuentaId,
        int reservaId,
        int millas)
    {
        if (cuentaId <= 0)
            throw new ArgumentException("La cuenta de millas es obligatoria.");

        if (reservaId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        return new MilesMovimiento
        {
            CuentaId    = cuentaId,
            ReservaId   = reservaId,
            Tipo        = TipoMilesMovimiento.Redencion(),
            Millas      = CantidadMilesMovimiento.Crear(millas),
            Fecha       = FechaMilesMovimiento.Ahora(),
            Descripcion = DescripcionMilesMovimiento.Crear(
                $"Redención de millas en Reserva #{reservaId}")
        };
    }

    // Ajuste manual por admin (corrección, bienvenida, compensación, etc.)
    public static MilesMovimiento PorAjusteManual(
        int cuentaId,
        int millas,
        string tipo,
        string motivo)
    {
        if (cuentaId <= 0)
            throw new ArgumentException("La cuenta de millas es obligatoria.");

        return new MilesMovimiento
        {
            CuentaId    = cuentaId,
            ReservaId   = null,
            Tipo        = TipoMilesMovimiento.Crear(tipo),
            Millas      = CantidadMilesMovimiento.Crear(millas),
            Fecha       = FechaMilesMovimiento.Ahora(),
            Descripcion = DescripcionMilesMovimiento.Crear($"Ajuste manual: {motivo}")
        };
    }

    // ── Fábrica de reconstitución (desde base de datos) ──────

    public static MilesMovimiento Reconstituir(
        int id,
        int cuentaId,
        int? reservaId,
        string tipo,
        int millas,
        DateTime fecha,
        string descripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del movimiento no es válido.");

        var movimiento = new MilesMovimiento
        {
            CuentaId    = cuentaId,
            ReservaId   = reservaId,
            Tipo        = TipoMilesMovimiento.Crear(tipo),
            Millas      = CantidadMilesMovimiento.Crear(millas),
            Fecha       = FechaMilesMovimiento.Crear(fecha),
            Descripcion = DescripcionMilesMovimiento.Crear(descripcion)
        };
        movimiento.Id = id;
        return movimiento;
    }

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del movimiento no es válido.");

        Id = id;
    }

    public override string ToString() =>
        $"[{Tipo}] {Millas} — {Fecha} | {Descripcion}";
}
