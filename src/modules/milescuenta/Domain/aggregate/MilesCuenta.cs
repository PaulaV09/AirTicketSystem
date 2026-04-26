// src/modules/milescuenta/Domain/aggregate/MilesCuenta.cs
using AirTicketSystem.modules.milescuenta.Domain.ValueObjects;

namespace AirTicketSystem.modules.milescuenta.Domain.aggregate;

public sealed class MilesCuenta
{
    public int Id { get; private set; }
    public int ClienteId { get; private set; }
    public SaldoMilesCuenta SaldoActual { get; private set; } = null!;

    // Total histórico acumulado (NO disminuye al redimir; determina el nivel)
    public int MilesAcumuladasTotal { get; private set; }
    public NivelMilesCuenta Nivel { get; private set; } = null!;
    public FechaInscripcionMilesCuenta FechaInscripcion { get; private set; } = null!;

    private MilesCuenta() { }

    // ── Fábrica de creación ──────────────────────────────────

    public static MilesCuenta Crear(int clienteId)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El cliente es obligatorio.");

        return new MilesCuenta
        {
            ClienteId            = clienteId,
            SaldoActual          = SaldoMilesCuenta.Cero(),
            MilesAcumuladasTotal = 0,
            Nivel                = NivelMilesCuenta.Calcular(0),
            FechaInscripcion     = FechaInscripcionMilesCuenta.Ahora()
        };
    }

    // ── Fábrica de reconstitucion (desde base de datos) ──────

    public static MilesCuenta Reconstituir(
        int id,
        int clienteId,
        int saldoActual,
        int milesAcumuladasTotal,
        string nivel,
        DateTime fechaInscripcion)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la cuenta de millas no es válido.");

        var cuenta = new MilesCuenta
        {
            ClienteId            = clienteId,
            SaldoActual          = SaldoMilesCuenta.Crear(saldoActual),
            MilesAcumuladasTotal = milesAcumuladasTotal,
            Nivel                = NivelMilesCuenta.Crear(nivel),
            FechaInscripcion     = FechaInscripcionMilesCuenta.Crear(fechaInscripcion)
        };
        cuenta.Id = id;
        return cuenta;
    }

    // ── Operaciones de negocio ───────────────────────────────

    // Acumula millas: sube el saldo disponible Y el total histórico,
    // luego recalcula el nivel porque el total creció.
    public void AcumularMiles(int millas)
    {
        if (millas <= 0)
            throw new ArgumentException(
                "La cantidad de millas a acumular debe ser mayor a 0.");

        var nuevoSaldo = SaldoActual.Valor + millas;
        var nuevoTotal = MilesAcumuladasTotal + millas;

        SaldoActual          = SaldoMilesCuenta.Crear(nuevoSaldo);
        MilesAcumuladasTotal = nuevoTotal;

        // El nivel solo sube; se recalcula con el total histórico
        Nivel = NivelMilesCuenta.Calcular(MilesAcumuladasTotal);
    }

    // Redime millas: descuenta del saldo disponible.
    // El total histórico NO disminuye (las millas ya se ganaron).
    public void RedimirMiles(int millas)
    {
        if (millas <= 0)
            throw new ArgumentException(
                "La cantidad de millas a redimir debe ser mayor a 0.");

        if (!SaldoActual.TieneSuficientes(millas))
            throw new InvalidOperationException(
                $"Saldo insuficiente. Disponible: {SaldoActual.Valor:N0} millas, " +
                $"solicitado: {millas:N0} millas.");

        SaldoActual = SaldoMilesCuenta.Crear(SaldoActual.Valor - millas);
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool TieneMiles => SaldoActual.Valor > 0;

    // Convierte millas a valor en pesos: 1.000 millas = $1.000 COP
    // (inverso de la regla de acumulación: 1 milla por $1.000 COP)
    public decimal ValorEnPesos(int millas) => millas;

    // Cuántas millas le faltan para el siguiente nivel
    public int MilesParaSiguienteNivel =>
        Nivel.MilesParaSiguienteNivel(MilesAcumuladasTotal);

    public void EstablecerId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la cuenta de millas no es válido.");

        Id = id;
    }

    public override string ToString() =>
        $"Cuenta #{Id} — Cliente {ClienteId} | " +
        $"Saldo: {SaldoActual} | Nivel: {Nivel}";
}
