// src/modules/gate/Application/UseCases/DeactivateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class DeactivateGateUseCase
{
    private readonly IGateRepository _repository;

    public DeactivateGateUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var puerta = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");

        if (!puerta.Activa)
            throw new InvalidOperationException(
                $"La puerta '{puerta.Codigo}' ya se encuentra inactiva.");

        puerta.Activa = false;
        await _repository.UpdateAsync(puerta);
    }
}