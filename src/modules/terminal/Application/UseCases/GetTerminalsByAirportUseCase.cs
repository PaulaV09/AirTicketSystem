// src/modules/terminal/Application/UseCases/GetTerminalsByAirportUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class GetTerminalsByAirportUseCase
{
    private readonly ITerminalRepository _repository;

    public GetTerminalsByAirportUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TerminalEntity>> ExecuteAsync(int aeropuertoId)
    {
        if (aeropuertoId <= 0)
            throw new ArgumentException("El ID del aeropuerto no es válido.");

        return await _repository.GetByAeropuertoAsync(aeropuertoId);
    }
}