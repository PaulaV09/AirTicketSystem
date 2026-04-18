// src/modules/luggagetype/Domain/aggregate/LuggageType.cs
using AirTicketSystem.modules.luggagetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.luggagetype.Domain.aggregate;

public sealed class LuggageType
{
    public int Id { get; private set; }
    public NombreLuggageType Nombre { get; private set; } = null!;

    private LuggageType() { }

    public static LuggageType Crear(string nombre)
    {
        return new LuggageType
        {
            Nombre = NombreLuggageType.Crear(nombre)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreLuggageType.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}