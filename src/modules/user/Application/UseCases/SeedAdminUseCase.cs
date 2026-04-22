// src/modules/user/Application/UseCases/SeedAdminUseCase.cs
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class SeedAdminUseCase
{
    private readonly IRoleRepository   _roleRepository;
    private readonly IUserRepository   _userRepository;
    private readonly IPersonRepository _personRepository;

    public SeedAdminUseCase(
        IRoleRepository   roleRepository,
        IUserRepository   userRepository,
        IPersonRepository personRepository)
    {
        _roleRepository   = roleRepository;
        _userRepository   = userRepository;
        _personRepository = personRepository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // Garantizar rol ADMIN
        var roles = await _roleRepository.FindAllAsync();
        var rolAdmin = roles.FirstOrDefault(r => r.Nombre.Valor == "ADMIN");
        if (rolAdmin is null)
        {
            rolAdmin = Role.Crear("ADMIN");
            await _roleRepository.SaveAsync(rolAdmin);
        }

        // Garantizar rol CLIENTE
        var rolCliente = roles.FirstOrDefault(r => r.Nombre.Valor == "CLIENTE");
        if (rolCliente is null)
        {
            rolCliente = Role.Crear("CLIENTE");
            await _roleRepository.SaveAsync(rolCliente);
        }

        // Garantizar usuario admin
        if (await _userRepository.ExistsByUsernameAsync("admin"))
            return;

        // Crear persona mínima para el admin (tipoDocId=1 = Cédula por convención)
        var persona = Person.Crear(
            tipoDocId: 1,
            numeroDoc: "0000000000",
            nombres:   "Administrador",
            apellidos: "Sistema");
        await _personRepository.SaveAsync(persona);

        // Hash de "admin123"
        var hash = PasswordHasher.Hash("admin123");
        var admin = User.Crear(persona.Id, rolAdmin.Id, "admin", hash);
        await _userRepository.SaveAsync(admin);
    }
}
