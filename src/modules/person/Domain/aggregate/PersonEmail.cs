// src/modules/person/Domain/aggregate/PersonEmail.cs
using AirTicketSystem.modules.person.Domain.ValueObjects;

namespace AirTicketSystem.modules.person.Domain.aggregate;

public sealed class PersonEmail
{
    public int Id { get; private set; }
    public int PersonaId { get; private set; }
    public int TipoEmailId { get; private set; }
    public EmailPersonEmail Email { get; private set; } = null!;
    public EsPrincipalPersonEmail EsPrincipal { get; private set; } = null!;

    private PersonEmail() { }

    public static PersonEmail Crear(
        int personaId,
        int tipoEmailId,
        string email,
        bool esPrincipal = false)
    {
        if (personaId <= 0)
            throw new ArgumentException("La persona es obligatoria.");

        if (tipoEmailId <= 0)
            throw new ArgumentException("El tipo de email es obligatorio.");

        return new PersonEmail
        {
            PersonaId   = personaId,
            TipoEmailId = tipoEmailId,
            Email       = EmailPersonEmail.Crear(email),
            EsPrincipal = EsPrincipalPersonEmail.Crear(esPrincipal)
        };
    }

    public void MarcarComoPrincipal()
    {
        EsPrincipal = EsPrincipalPersonEmail.Principal();
    }

    public void MarcarComoSecundario()
    {
        EsPrincipal = EsPrincipalPersonEmail.Secundario();
    }

    public void ActualizarEmail(string email)
    {
        Email = EmailPersonEmail.Crear(email);
    }

    public override string ToString() => Email.ToString();
}