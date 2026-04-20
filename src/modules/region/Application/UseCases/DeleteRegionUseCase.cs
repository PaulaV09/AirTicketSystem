// src/modules/region/Application/UseCases/DeleteRegionUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class DeleteRegionUseCase
{
    private readonly IRegionRepository _repository;

    public DeleteRegionUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}