// src/modules/client/Application/UseCases/CreateClientUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class CreateClientUseCase
{
    private readonly IClientRepository      _repository;
    private readonly IPersonRepository      _personRepository;
    private readonly IUserRepository        _userRepository;
    private readonly IMilesCuentaRepository _cuentaMilesRepository;

    public CreateClientUseCase(
        IClientRepository      repository,
        IPersonRepository      personRepository,
        IUserRepository        userRepository,
        IMilesCuentaRepository cuentaMilesRepository)
    {
        _repository            = repository;
        _personRepository      = personRepository;
        _userRepository        = userRepository;
        _cuentaMilesRepository = cuentaMilesRepository;
    }

    public async Task<Client> ExecuteAsync(
        int personaId,
        int usuarioId,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        var usuario = await _userRepository.FindByIdAsync(usuarioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {usuarioId}.");

        if (usuario.EstaBloqueado)
            throw new InvalidOperationException(
                "No se puede crear un cliente con un usuario bloqueado o inactivo.");

        if (await _repository.FindByPersonaAsync(personaId) is not null)
            throw new InvalidOperationException(
                "Esta persona ya tiene un registro como cliente.");

        if (await _repository.FindByUsuarioAsync(usuarioId) is not null)
            throw new InvalidOperationException(
                "Este usuario ya está asociado a un cliente.");

        var client = Client.Crear(personaId, usuarioId);
        await _repository.SaveAsync(client);

        // Crear la cuenta de millas automáticamente junto con el cliente
        if (!await _cuentaMilesRepository.ExistsByClienteAsync(client.Id))
        {
            var cuenta = MilesCuenta.Crear(client.Id);
            await _cuentaMilesRepository.SaveAsync(cuenta);
        }

        return client;
    }
}
