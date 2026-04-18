// src/modules/emailtype/Domain/aggregate/EmailType.cs
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Domain.aggregate;

public sealed class EmailType
{
    public int Id { get; private set; }
    public DescripcionEmailType Descripcion { get; private set; } = null!;

    private EmailType() { }

    public static EmailType Crear(string descripcion)
    {
        return new EmailType
        {
            Descripcion = DescripcionEmailType.Crear(descripcion)
        };
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionEmailType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}