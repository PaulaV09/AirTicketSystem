// src/modules/specialty/Domain/aggregate/Specialty.cs
using AirTicketSystem.modules.specialty.Domain.ValueObjects;

namespace AirTicketSystem.modules.specialty.Domain.aggregate;

public sealed class Specialty
{
    public int Id { get; private set; }
    public int? TipoTrabajadorId { get; private set; }
    public NombreSpecialty Nombre { get; private set; } = null!;

    private Specialty() { }

    public static Specialty Crear(string nombre, int? tipoTrabajadorId = null)
    {
        if (tipoTrabajadorId.HasValue && tipoTrabajadorId <= 0)
            throw new ArgumentException("El tipo de trabajador no es válido.");

        return new Specialty
        {
            Nombre            = NombreSpecialty.Crear(nombre),
            TipoTrabajadorId  = tipoTrabajadorId
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreSpecialty.Crear(nombre);
    }

    public void AsignarTipoTrabajador(int? tipoTrabajadorId)
    {
        if (tipoTrabajadorId.HasValue && tipoTrabajadorId <= 0)
            throw new ArgumentException("El tipo de trabajador no es válido.");

        TipoTrabajadorId = tipoTrabajadorId;
    }

    // Propiedades de negocio
    public bool EsEspecialidadGeneral => TipoTrabajadorId is null;

    public override string ToString() => Nombre.ToString();
}