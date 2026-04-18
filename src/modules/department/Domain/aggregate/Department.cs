// src/modules/department/Domain/aggregate/Department.cs
using AirTicketSystem.modules.department.Domain.ValueObjects;

namespace AirTicketSystem.modules.department.Domain.aggregate;

public sealed class Department
{
    public int Id { get; private set; }
    public int RegionId { get; private set; }
    public NombreDepartment Nombre { get; private set; } = null!;
    public CodigoDepartment? Codigo { get; private set; }

    private Department() { }

    public static Department Crear(int regionId, string nombre, string? codigo = null)
    {
        if (regionId <= 0)
            throw new ArgumentException("La región es obligatoria.");

        return new Department
        {
            RegionId = regionId,
            Nombre   = NombreDepartment.Crear(nombre),
            Codigo   = codigo is not null ? CodigoDepartment.Crear(codigo) : null
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreDepartment.Crear(nombre);
    }

    public void ActualizarCodigo(string? codigo)
    {
        Codigo = codigo is not null ? CodigoDepartment.Crear(codigo) : null;
    }

    public override string ToString() =>
        Codigo is not null ? $"[{Codigo}] {Nombre}" : Nombre.ToString();
}