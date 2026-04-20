// src/modules/emailtype/Application/UseCases/DeleteEmailTypeUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.Repositories;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public class DeleteEmailTypeUseCase
{
    private readonly IEmailTypeRepository _repository;

    public DeleteEmailTypeUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}