// src/modules/worker/Application/UseCases/UpdateWorkerAirportUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class UpdateWorkerAirportUseCase
{
    private readonly IWorkerRepository _repository;
    private readonly IAirportRepository _airportRepository;

    public UpdateWorkerAirportUseCase(
        IWorkerRepository repository,
        IAirportRepository airportRepository)
    {
        _repository        = repository;
        _airportRepository = airportRepository;
    }

    public async Task<WorkerEntity> ExecuteAsync(int id, int aeropuertoBaseId)
    {
        var trabajador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (!trabajador.Activo)
            throw new InvalidOperationException(
                "No se puede cambiar el aeropuerto base de un trabajador inactivo.");

        if (aeropuertoBaseId == trabajador.AeropuertoBaseId)
            throw new InvalidOperationException(
                "El aeropuerto indicado es el mismo que el aeropuerto base actual.");

        var aeropuerto = await _airportRepository.GetByIdAsync(aeropuertoBaseId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {aeropuertoBaseId}.");

        if (!aeropuerto.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre}' está inactivo.");

        trabajador.AeropuertoBaseId = aeropuertoBaseId;
        await _repository.UpdateAsync(trabajador);
        return trabajador;
    }
}