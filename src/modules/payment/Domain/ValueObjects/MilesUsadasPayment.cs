// src/modules/payment/Domain/ValueObjects/MilesUsadasPayment.cs
namespace AirTicketSystem.modules.payment.Domain.ValueObjects;

public sealed class MilesUsadasPayment
{
    public int Valor { get; }

    private MilesUsadasPayment(int valor) => Valor = valor;

    public static MilesUsadasPayment Crear(int valor)
    {
        if (valor <= 0)
            throw new ArgumentException(
                "La cantidad de millas usadas debe ser mayor a 0.");

        return new MilesUsadasPayment(valor);
    }

    // Regla de conversión: 1 milla = $1 COP
    // (inverso de la acumulación: 1 milla por $1.000 COP → valor en pesos = millas)
    public decimal ValorEnPesos => Valor;

    public override string ToString() => $"{Valor:N0} millas";
}
