// src/modules/milesmovimiento/Domain/ValueObjects/CantidadMilesMovimiento.cs
namespace AirTicketSystem.modules.milesmovimiento.Domain.ValueObjects;

public sealed class CantidadMilesMovimiento
{
    public int Valor { get; }

    private CantidadMilesMovimiento(int valor) => Valor = valor;

    public static CantidadMilesMovimiento Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "La cantidad de millas de un movimiento debe ser mayor a 0.");

        if (valor > 10_000_000)
            throw new ArgumentException(
                "La cantidad de millas no puede superar 10.000.000 por movimiento.");

        return new CantidadMilesMovimiento(valor);
    }

    public override string ToString() => $"{Valor:N0} millas";
}
