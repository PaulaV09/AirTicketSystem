// src/modules/airline/Domain/aggregate/Airline.cs
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Domain.aggregate;

public sealed class Airline
{
    public int Id { get; private set; }
    public int PaisId { get; private set; }
    public CodigoIataAerolinea CodigoIata { get; private set; } = null!;
    public CodigoIcaoAerolinea CodigoIcao { get; private set; } = null!;
    public NombreAerolinea Nombre { get; private set; } = null!;
    public NombreComercialAerolinea? NombreComercial { get; private set; }
    public SitioWebAerolinea? SitioWeb { get; private set; }
    public ActivaAerolinea Activa { get; private set; } = null!;

    private Airline() { }

    public static Airline Crear(
        int paisId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? nombreComercial = null,
        string? sitioWeb = null)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país de la aerolínea es obligatorio.");

        return new Airline
        {
            PaisId          = paisId,
            CodigoIata      = CodigoIataAirline.Crear(codigoIata),
            CodigoIcao      = CodigoIcaoAirline.Crear(codigoIcao),
            Nombre          = NombreAirline.Crear(nombre),
            NombreComercial = nombreComercial is not null
                ? NombreComercialAirline.Crear(nombreComercial)
                : null,
            SitioWeb = sitioWeb is not null
                ? SitioWebAirline.Crear(sitioWeb)
                : null,
            Activa = ActivaAirline.Activa()
        };
    }

    public void ActualizarNombre(string nombre, string? nombreComercial = null)
    {
        Nombre = NombreAirline.Crear(nombre);
        NombreComercial = nombreComercial is not null
            ? NombreComercialAirline.Crear(nombreComercial)
            : null;
    }

    public void ActualizarSitioWeb(string? sitioWeb)
    {
        SitioWeb = sitioWeb is not null
            ? SitioWebAirline.Crear(sitioWeb)
            : null;
    }

    public void ActualizarPais(int paisId)
    {
        if (paisId <= 0)
            throw new ArgumentException("El país es obligatorio.");

        PaisId = paisId;
    }

    public void Activar()
    {
        if (Activa.Valor)
            throw new InvalidOperationException(
                "La aerolínea ya se encuentra activa.");

        Activa = ActivaAirline.Activa();
    }

    public void Desactivar()
    {
        if (!Activa.Valor)
            throw new InvalidOperationException(
                "La aerolínea ya se encuentra inactiva.");

        Activa = ActivaAirline.Inactiva();
    }

    // Propiedades de negocio
    public bool EstaOperativa => Activa.Valor;

    public string NombreParaMostrar =>
        NombreComercial is not null
            ? NombreComercial.Valor
            : Nombre.Valor;

    public override string ToString() =>
        $"[{CodigoIata}] {NombreParaMostrar}";
}