// src/modules/terminal/Application/Services/TerminalService.cs
using AirTicketSystem.modules.terminal.Application.Interfaces;
using AirTicketSystem.modules.terminal.Application.UseCases;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Application.Services;

public class TerminalService : ITerminalService
{
    private readonly CreateTerminalUseCase _create;
    private readonly GetTerminalByIdUseCase _getById;
    private readonly GetTerminalsByAirportUseCase _getByAirport;
    private readonly UpdateTerminalUseCase _update;
    private readonly DeleteTerminalUseCase _delete;

    public TerminalService(
        CreateTerminalUseCase create,
        GetTerminalByIdUseCase getById,
        GetTerminalsByAirportUseCase getByAirport,
        UpdateTerminalUseCase update,
        DeleteTerminalUseCase delete)
    {
        _create       = create;
        _getById      = getById;
        _getByAirport = getByAirport;
        _update       = update;
        _delete       = delete;
    }

    public Task<TerminalEntity> CreateAsync(
        int aeropuertoId, string nombre, string? descripcion)
        => _create.ExecuteAsync(aeropuertoId, nombre, descripcion);

    public Task<TerminalEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<TerminalEntity>> GetByAirportAsync(int aeropuertoId)
        => _getByAirport.ExecuteAsync(aeropuertoId);

    public Task<TerminalEntity> UpdateAsync(
        int id, string nombre, string? descripcion)
        => _update.ExecuteAsync(id, nombre, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}