// src/modules/worker/Application/UseCases/GetWorkersByAirlineUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetWorkersByAirlineUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByAirlineUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync(int aerolineaId)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.GetByAerolineaAsync(aerolineaId);
    }
}