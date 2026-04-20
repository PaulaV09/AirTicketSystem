// src/modules/gate/Application/UseCases/GetActiveGatesByTerminalUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class GetActiveGatesByTerminalUseCase
{
    private readonly IGateRepository _repository;

    public GetActiveGatesByTerminalUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GateEntity>> ExecuteAsync(int terminalId)
    {
        if (terminalId <= 0)
            throw new ArgumentException("El ID de la terminal no es válido.");

        return await _repository.GetActivasByTerminalAsync(terminalId);
    }
}