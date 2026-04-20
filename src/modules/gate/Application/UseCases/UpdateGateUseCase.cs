// src/modules/gate/Application/UseCases/UpdateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Infrastructure.entity;
using AirTicketSystem.modules.gate.Domain.ValueObjects;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class UpdateGateUseCase
{
    private readonly IGateRepository _repository;

    public UpdateGateUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task<GateEntity> ExecuteAsync(int id, string codigo)
    {
        var puerta = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");

        var codigoVO = CodigoGate.Crear(codigo);

        if (codigoVO.Valor != puerta.Codigo &&
            await _repository.ExistsByCodigoAndTerminalAsync(
                codigoVO.Valor, puerta.TerminalId))
            throw new InvalidOperationException(
                $"Ya existe otra puerta con el código '{codigoVO.Valor}' " +
                "en la misma terminal.");

        puerta.Codigo = codigoVO.Valor;
        await _repository.UpdateAsync(puerta);
        return puerta;
    }
}