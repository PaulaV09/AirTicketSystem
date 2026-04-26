// src/modules/milescuenta/Domain/ValueObjects/SaldoMilesCuenta.cs
namespace AirTicketSystem.modules.milescuenta.Domain.ValueObjects;

public sealed class SaldoMilesCuenta
{
    public int Valor { get; }

    private SaldoMilesCuenta(int valor) => Valor = valor;

    public static SaldoMilesCuenta Crear(int valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El saldo de millas no puede ser negativo.");

        return new SaldoMilesCuenta(valor);
    }

    public static SaldoMilesCuenta Cero() => new(0);

    // Verifica si hay millas suficientes para redimir
    public bool TieneSuficientes(int millas) => Valor >= millas;

    public override string ToString() => $"{Valor:N0} millas";
}
