// src/modules/addresstype/Domain/aggregate/AddressType.cs
using AirTicketSystem.modules.addresstype.Domain.ValueObjects;

namespace AirTicketSystem.modules.addresstype.Domain.aggregate;

public sealed class AddressType
{
    public int Id { get; private set; }
    public DescripcionAddressType Descripcion { get; private set; } = null!;

    private AddressType() { }

    public static AddressType Crear(string descripcion)
    {
        return new AddressType
        {
            Descripcion = DescripcionAddressType.Crear(descripcion)
        };
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionAddressType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}