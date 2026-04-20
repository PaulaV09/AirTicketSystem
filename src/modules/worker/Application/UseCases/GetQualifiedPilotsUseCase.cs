// src/modules/worker/Application/UseCases/GetQualifiedPilotsUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetQualifiedPilotsUseCase
{
    private readonly IWorkerRepository _repository;

    public GetQualifiedPilotsUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync(int modeloAvionId)
    {
        if (modeloAvionId <= 0)
            throw new ArgumentException(
                "El ID del modelo de avión no es válido.");

        var pilotos = await _repository
            .GetPilotosHabilitadosParaModeloAsync(modeloAvionId);

        if (!pilotos.Any())
            throw new InvalidOperationException(
                $"No hay pilotos habilitados para el modelo de avión " +
                $"con ID {modeloAvionId}.");

        return pilotos;
    }
}