// src/modules/paymentmethod/Domain/aggregate/PaymentMethod.cs
using AirTicketSystem.modules.paymentmethod.Domain.ValueObjects;

namespace AirTicketSystem.modules.paymentmethod.Domain.aggregate;

public sealed class PaymentMethod
{
    public int Id { get; private set; }
    public NombrePaymentMethod Nombre { get; private set; } = null!;

    private PaymentMethod() { }

    public static PaymentMethod Crear(string nombre)
    {
        return new PaymentMethod
        {
            Nombre = NombrePaymentMethod.Crear(nombre)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombrePaymentMethod.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}