// src/modules/region/Domain/aggregate/Region.cs
using AirTicketSystem.modules.region.Domain.ValueObjects;

namespace AirTicketSystem.modules.region.Domain.aggregate;

public sealed class Region
{
    public int Id { get; private set; }
    public int PaisId { get; private set; }
    public NombreRegion Nombre { get; private set; } = null!;
    public CodigoRegion? Codigo { get; private set; }

    private Region() { }

    public static Region Crear(int paisId, string nombre, string? codigo = null)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país es obligatorio.");

        return new Region
        {
            PaisId = paisId,
            Nombre = NombreRegion.Crear(nombre),
            Codigo = codigo is not null ? CodigoRegion.Crear(codigo) : null
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreRegion.Crear(nombre);
    }

    public void ActualizarCodigo(string? codigo)
    {
        Codigo = codigo is not null ? CodigoRegion.Crear(codigo) : null;
    }

    public override string ToString() =>
        Codigo is not null ? $"[{Codigo}] {Nombre}" : Nombre.ToString();
}