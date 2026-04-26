// src/modules/milesmovimiento/Domain/ValueObjects/DescripcionMilesMovimiento.cs
namespace AirTicketSystem.modules.milesmovimiento.Domain.ValueObjects;

public sealed class DescripcionMilesMovimiento
{
    public string Valor { get; }

    private DescripcionMilesMovimiento(string valor) => Valor = valor;

    public static DescripcionMilesMovimiento Crear(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException(
                "La descripción del movimiento de millas es obligatoria.");

        if (valor.Length > 200)
            throw new ArgumentException(
                "La descripción no puede superar 200 caracteres.");

        return new DescripcionMilesMovimiento(valor.Trim());
    }

    public override string ToString() => Valor;
}
