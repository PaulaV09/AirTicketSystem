// src/modules/role/Domain/aggregate/Role.cs
using AirTicketSystem.modules.role.Domain.ValueObjects;

namespace AirTicketSystem.modules.role.Domain.aggregate;

public sealed class Role
{
    public int Id { get; private set; }
    public NombreRole Nombre { get; private set; } = null!;

    private Role() { }

    public static Role Crear(string nombre)
    {
        return new Role
        {
            Nombre = NombreRole.Crear(nombre)
        };
    }

    public void ActualizarNombre(string nombre)
    {
        Nombre = NombreRole.Crear(nombre);
    }

    public override string ToString() => Nombre.ToString();
}