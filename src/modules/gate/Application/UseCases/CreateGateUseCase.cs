// src/modules/gate/Application/UseCases/CreateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Infrastructure.entity;
using AirTicketSystem.modules.gate.Domain.ValueObjects;
using AirTicketSystem.modules.terminal.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class CreateGateUseCase
{
    private readonly IGateRepository _repository;
    private readonly ITerminalRepository _terminalRepository;

    public CreateGateUseCase(
        IGateRepository repository,
        ITerminalRepository terminalRepository)
    {
        _repository         = repository;
        _terminalRepository = terminalRepository;
    }

    public async Task<GateEntity> ExecuteAsync(int terminalId, string codigo)
    {
        _ = await _terminalRepository.GetByIdAsync(terminalId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {terminalId}.");

        var codigoVO = CodigoGate.Crear(codigo);

        if (await _repository.ExistsByCodigoAndTerminalAsync(
            codigoVO.Valor, terminalId))
            throw new InvalidOperationException(
                $"Ya existe una puerta con el código '{codigoVO.Valor}' " +
                $"en la terminal con ID {terminalId}.");

        var entity = new GateEntity
        {
            TerminalId = terminalId,
            Codigo     = codigoVO.Valor,
            Activa     = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}