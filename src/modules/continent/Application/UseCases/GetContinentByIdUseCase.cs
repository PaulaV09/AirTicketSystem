// src/modules/continent/Application/UseCases/GetContinentByIdUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public class GetContinentByIdUseCase
{
    private readonly IContinentRepository _repository;

    public GetContinentByIdUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContinentEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        var continente = await _repository.GetByIdAsync(id);

        if (continente is null)
            throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");

        return continente;
    }
}