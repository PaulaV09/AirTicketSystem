// src/modules/country/Application/UseCases/DeleteCountryUseCase.cs
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.country.Application.UseCases;

public class DeleteCountryUseCase
{
    private readonly ICountryRepository _repository;

    public DeleteCountryUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}