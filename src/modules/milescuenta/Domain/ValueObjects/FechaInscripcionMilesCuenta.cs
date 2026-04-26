// src/modules/milescuenta/Domain/ValueObjects/FechaInscripcionMilesCuenta.cs
namespace AirTicketSystem.modules.milescuenta.Domain.ValueObjects;

public sealed class FechaInscripcionMilesCuenta
{
    public DateTime Valor { get; }

    private FechaInscripcionMilesCuenta(DateTime valor) => Valor = valor;

    public static FechaInscripcionMilesCuenta Ahora() =>
        new(DateTime.UtcNow);

    public static FechaInscripcionMilesCuenta Crear(DateTime valor) =>
        new(valor);

    public int DiasInscrito =>
        (int)(DateTime.UtcNow - Valor).TotalDays;

    public override string ToString() =>
        Valor.ToString("yyyy-MM-dd");
}
