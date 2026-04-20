// src/modules/terminal/Application/UseCases/UpdateTerminalUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Infrastructure.entity;
using AirTicketSystem.modules.terminal.Domain.ValueObjects;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class UpdateTerminalUseCase
{
    private readonly ITerminalRepository _repository;

    public UpdateTerminalUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<TerminalEntity> ExecuteAsync(
        int id, string nombre, string? descripcion)
    {
        var terminal = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {id}.");

        var nombreVO = NombreTerminal.Crear(nombre);

        if (nombreVO.Valor != terminal.Nombre &&
            await _repository.ExistsByNombreAndAeropuertoAsync(
                nombreVO.Valor, terminal.AeropuertoId))
            throw new InvalidOperationException(
                $"Ya existe otra terminal con el nombre '{nombreVO.Valor}' " +
                "en el mismo aeropuerto.");

        terminal.Nombre      = nombreVO.Valor;
        terminal.Descripcion = descripcion is not null
            ? DescripcionTerminal.Crear(descripcion).Valor
            : null;

        await _repository.UpdateAsync(terminal);
        return terminal;
    }
}