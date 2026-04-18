// src/modules/continent/Domain/aggregate/Continent.cs
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Domain.aggregate;

public sealed class Continent
{
    public int Id { get; private set; }
    public NombreContinent Nombre { get; private set; } = null!;
    public CodigoContinent Codigo { get; private set; } = null!;

    private Continent() { }

    public static Continent Crear(string nombre, string codigo)
    {
        return new Continent
        {
            Nombre = NombreContinent.Crear(nombre),
            Codigo = CodigoContinent.Crear(codigo)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreContinent.Crear(nombre);
    }

    public void ActualizarCodigo(string codigo)
    {
        Codigo = CodigoContinent.Crear(codigo);
    }

    public override string ToString() =>
        $"[{Codigo}] {Nombre}";
}