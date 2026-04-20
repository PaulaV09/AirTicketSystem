// src/modules/worker/Application/UseCases/CreateWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.worker.Domain.ValueObjects;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class CreateWorkerUseCase
{
    private readonly IWorkerRepository _repository;
    private readonly IPersonRepository _personRepository;
    private readonly IWorkerTypeRepository _workerTypeRepository;
    private readonly IAirportRepository _airportRepository;
    private readonly IAirlineRepository _airlineRepository;

    public CreateWorkerUseCase(
        IWorkerRepository repository,
        IPersonRepository personRepository,
        IWorkerTypeRepository workerTypeRepository,
        IAirportRepository airportRepository,
        IAirlineRepository airlineRepository)
    {
        _repository          = repository;
        _personRepository    = personRepository;
        _workerTypeRepository = workerTypeRepository;
        _airportRepository   = airportRepository;
        _airlineRepository   = airlineRepository;
    }

    public async Task<WorkerEntity> ExecuteAsync(
        int personaId,
        int tipoTrabajadorId,
        int aeropuertoBaseId,
        DateOnly fechaContratacion,
        decimal salario,
        int? aerolineaId,
        int? usuarioId)
    {
        _ = await _personRepository.GetByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        // Verificar que la persona no sea ya un trabajador
        var existente = await _repository.GetByPersonaAsync(personaId);
        if (existente is not null)
            throw new InvalidOperationException(
                "Esta persona ya tiene un registro como trabajador.");

        _ = await _workerTypeRepository.GetByIdAsync(tipoTrabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {tipoTrabajadorId}.");

        var aeropuerto = await _airportRepository.GetByIdAsync(aeropuertoBaseId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {aeropuertoBaseId}.");

        if (!aeropuerto.Activo)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre}' está inactivo.");

        if (aerolineaId.HasValue)
        {
            var aerolinea = await _airlineRepository.GetByIdAsync(aerolineaId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró una aerolínea con ID {aerolineaId.Value}.");

            if (!aerolinea.Activa)
                throw new InvalidOperationException(
                    $"La aerolínea '{aerolinea.Nombre}' está inactiva.");
        }

        var fechaVO  = FechaContratacionWorker.Crear(fechaContratacion);
        var salarioVO = SalarioWorker.Crear(salario);

        var entity = new WorkerEntity
        {
            PersonaId        = personaId,
            TipoTrabajadorId = tipoTrabajadorId,
            AeropuertoBaseId = aeropuertoBaseId,
            AerolineaId      = aerolineaId,
            UsuarioId        = usuarioId,
            FechaContratacion = fechaVO.Valor,
            Salario          = salarioVO.Valor,
            Activo           = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}