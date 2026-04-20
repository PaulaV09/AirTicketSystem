// src/modules/worker/Application/UseCases/AssignUserToWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class AssignUserToWorkerUseCase
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IUserRepository _userRepository;

    public AssignUserToWorkerUseCase(
        IWorkerRepository workerRepository,
        IUserRepository userRepository)
    {
        _workerRepository = workerRepository;
        _userRepository   = userRepository;
    }

    public async Task ExecuteAsync(int trabajadorId, int usuarioId)
    {
        var trabajador = await _workerRepository.GetByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (trabajador.UsuarioId.HasValue)
            throw new InvalidOperationException(
                "El trabajador ya tiene un usuario asignado. " +
                "No se puede reasignar el usuario directamente.");

        var usuario = await _userRepository.GetByIdAsync(usuarioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {usuarioId}.");

        if (!usuario.Activo)
            throw new InvalidOperationException(
                "No se puede asignar un usuario inactivo a un trabajador.");

        trabajador.UsuarioId = usuarioId;
        await _workerRepository.UpdateAsync(trabajador);
    }
}