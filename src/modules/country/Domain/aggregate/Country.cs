// src/modules/country/Domain/aggregate/Country.cs
using AirTicketSystem.modules.country.Domain.ValueObjects;

namespace AirTicketSystem.modules.country.Domain.aggregate;

public sealed class Country
{
    public int Id { get; private set; }
    public int ContinenteId { get; private set; }
    public NombreCountry Nombre { get; private set; } = null!;
    public CodigoIso2Country CodigoIso2 { get; private set; } = null!;
    public CodigoIso3Country CodigoIso3 { get; private set; } = null!;

    private Country() { }

    public static Country Crear(
        int continenteId,
        string nombre,
        string codigoIso2,
        string codigoIso3)
    {
        if (continenteId <= 0)
            throw new ArgumentException("El continente es obligatorio.");

        return new Country
        {
            ContinenteId = continenteId,
            Nombre       = NombreCountry.Crear(nombre),
            CodigoIso2   = CodigoIso2Country.Crear(codigoIso2),
            CodigoIso3   = CodigoIso3Country.Crear(codigoIso3)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreCountry.Crear(nombre);
    }

    public override string ToString() =>
        $"[{CodigoIso2}] {Nombre}";
}