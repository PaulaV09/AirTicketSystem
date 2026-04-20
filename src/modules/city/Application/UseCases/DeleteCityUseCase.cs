// src/modules/city/Application/UseCases/DeleteCityUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class DeleteCityUseCase
{
    private readonly ICityRepository _repository;

    public DeleteCityUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}