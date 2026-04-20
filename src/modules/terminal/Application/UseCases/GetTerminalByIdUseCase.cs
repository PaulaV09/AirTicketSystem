// src/modules/terminal/Application/UseCases/GetTerminalByIdUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class GetTerminalByIdUseCase
{
    private readonly ITerminalRepository _repository;

    public GetTerminalByIdUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<TerminalEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la terminal no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {id}.");
    }
}