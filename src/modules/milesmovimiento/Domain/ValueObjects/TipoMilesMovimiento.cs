// src/modules/milesmovimiento/Domain/ValueObjects/TipoMilesMovimiento.cs
namespace AirTicketSystem.modules.milesmovimiento.Domain.ValueObjects;

public sealed class TipoMilesMovimiento
{
    public string Valor { get; }

    private static readonly string[] TiposValidos = ["ACUMULACION", "REDENCION"];

    private TipoMilesMovimiento(string valor) => Valor = valor;

    public static TipoMilesMovimiento Acumulacion() => new("ACUMULACION");

    public static TipoMilesMovimiento Redencion() => new("REDENCION");

    public static TipoMilesMovimiento Crear(string valor)
    {
        var normalizado = valor.ToUpperInvariant();
        if (!TiposValidos.Contains(normalizado))
            throw new ArgumentException(
                $"Tipo de movimiento '{valor}' no es válido. " +
                $"Valores aceptados: {string.Join(", ", TiposValidos)}.");

        return new TipoMilesMovimiento(normalizado);
    }

    public bool EsAcumulacion => Valor == "ACUMULACION";
    public bool EsRedencion   => Valor == "REDENCION";

    public override string ToString() => Valor;
}
