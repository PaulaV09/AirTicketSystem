// src/modules/milesmovimiento/Domain/ValueObjects/FechaMilesMovimiento.cs
namespace AirTicketSystem.modules.milesmovimiento.Domain.ValueObjects;

public sealed class FechaMilesMovimiento
{
    public DateTime Valor { get; }

    private FechaMilesMovimiento(DateTime valor) => Valor = valor;

    public static FechaMilesMovimiento Ahora() => new(DateTime.UtcNow);

    public static FechaMilesMovimiento Crear(DateTime valor) => new(valor);

    public override string ToString() => Valor.ToString("yyyy-MM-dd HH:mm");
}
