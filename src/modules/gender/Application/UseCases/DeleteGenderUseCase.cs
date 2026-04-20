// src/modules/gender/Application/UseCases/DeleteGenderUseCase.cs
using AirTicketSystem.modules.gender.Domain.Repositories;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public class DeleteGenderUseCase
{
    private readonly IGenderRepository _repository;

    public DeleteGenderUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un género con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}