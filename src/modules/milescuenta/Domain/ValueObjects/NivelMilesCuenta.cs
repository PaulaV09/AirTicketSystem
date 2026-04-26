// src/modules/milescuenta/Domain/ValueObjects/NivelMilesCuenta.cs
namespace AirTicketSystem.modules.milescuenta.Domain.ValueObjects;

public sealed class NivelMilesCuenta
{
    public string Valor { get; }

    private static readonly string[] NivelesValidos = ["BRONCE", "PLATA", "ORO", "PLATINO"];

    private NivelMilesCuenta(string valor) => Valor = valor;

    // Fábrica principal: calcula el nivel según millas históricas totales
    // Regla: BRONCE < 5.000 | PLATA 5.000-19.999 | ORO 20.000-49.999 | PLATINO >= 50.000
    public static NivelMilesCuenta Calcular(int milesAcumuladasTotal)
    {
        var nivel = milesAcumuladasTotal switch
        {
            >= 50_000 => "PLATINO",
            >= 20_000 => "ORO",
            >= 5_000  => "PLATA",
            _         => "BRONCE"
        };
        return new NivelMilesCuenta(nivel);
    }

    // Fábrica para reconstituir desde base de datos
    public static NivelMilesCuenta Crear(string valor)
    {
        var normalizado = valor.ToUpperInvariant();
        if (!NivelesValidos.Contains(normalizado))
            throw new ArgumentException(
                $"Nivel de millas '{valor}' no es válido. " +
                $"Valores aceptados: {string.Join(", ", NivelesValidos)}.");

        return new NivelMilesCuenta(normalizado);
    }

    // Millas necesarias para subir al siguiente nivel
    public int MilesParaSiguienteNivel(int milesAcumuladasTotal) =>
        Valor switch
        {
            "BRONCE"  => 5_000  - milesAcumuladasTotal,
            "PLATA"   => 20_000 - milesAcumuladasTotal,
            "ORO"     => 50_000 - milesAcumuladasTotal,
            "PLATINO" => 0,
            _         => 0
        };

    public bool EsPlatino => Valor == "PLATINO";

    public override string ToString() => Valor;
}
