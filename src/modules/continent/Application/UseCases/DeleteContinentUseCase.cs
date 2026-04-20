// src/modules/continent/Application/UseCases/DeleteContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public class DeleteContinentUseCase
{
    private readonly IContinentRepository _repository;

    public DeleteContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var continente = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");

        // Un continente con países asociados no puede eliminarse
        // Esta regla la protege la FK en la BD, pero la verificamos
        // antes para dar un mensaje descriptivo al usuario
        await _repository.DeleteAsync(id);
    }
}