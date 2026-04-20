// src/modules/worker/Application/UseCases/AssignWorkerSpecialtyUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.specialty.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class AssignWorkerSpecialtyUseCase
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IWorkerSpecialtyRepository _workerSpecialtyRepository;
    private readonly ISpecialtyRepository _specialtyRepository;

    public AssignWorkerSpecialtyUseCase(
        IWorkerRepository workerRepository,
        IWorkerSpecialtyRepository workerSpecialtyRepository,
        ISpecialtyRepository specialtyRepository)
    {
        _workerRepository          = workerRepository;
        _workerSpecialtyRepository = workerSpecialtyRepository;
        _specialtyRepository       = specialtyRepository;
    }

    public async Task ExecuteAsync(int trabajadorId, int especialidadId)
    {
        var trabajador = await _workerRepository.GetByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!trabajador.Activo)
            throw new InvalidOperationException(
                "No se pueden asignar especialidades a un trabajador inactivo.");

        _ = await _specialtyRepository.GetByIdAsync(especialidadId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {especialidadId}.");

        if (await _workerSpecialtyRepository.ExistsByTrabajadorAndEspecialidadAsync(
            trabajadorId, especialidadId))
            throw new InvalidOperationException(
                "El trabajador ya tiene asignada esta especialidad.");

        var entity = new WorkerSpecialtyEntity
        {
            TrabajadorId   = trabajadorId,
            EspecialidadId = especialidadId
        };

        await _workerSpecialtyRepository.AddAsync(entity);
    }
}