// src/modules/gate/Application/UseCases/ActivateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class ActivateGateUseCase
{
    private readonly IGateRepository _repository;

    public ActivateGateUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var puerta = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");

        if (puerta.Activa)
            throw new InvalidOperationException(
                $"La puerta '{puerta.Codigo}' ya se encuentra activa.");

        puerta.Activa = true;
        await _repository.UpdateAsync(puerta);
    }
}