// src/modules/worker/Application/UseCases/GetWorkerByPersonUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetWorkerByPersonUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkerByPersonUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerEntity> ExecuteAsync(int personaId)
    {
        if (personaId <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        return await _repository.GetByPersonaAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador asociado a la persona con ID {personaId}.");
    }
}