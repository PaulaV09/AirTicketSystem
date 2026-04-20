// src/modules/worker/Application/UseCases/GetWorkersByWorkerTypeUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetWorkersByWorkerTypeUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByWorkerTypeUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync(int tipoTrabajadorId)
    {
        if (tipoTrabajadorId <= 0)
            throw new ArgumentException(
                "El ID del tipo de trabajador no es válido.");

        return await _repository.GetByTipoTrabajadorAsync(tipoTrabajadorId);
    }
}