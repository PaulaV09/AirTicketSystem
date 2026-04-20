// src/modules/worker/Application/UseCases/GetWorkersByAirportUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class GetWorkersByAirportUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByAirportUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<WorkerEntity>> ExecuteAsync(int aeropuertoId)
    {
        if (aeropuertoId <= 0)
            throw new ArgumentException("El ID del aeropuerto no es válido.");

        return await _repository.GetByAeropuertoBaseAsync(aeropuertoId);
    }
}