// src/modules/aircraft/Application/UseCases/CreateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class CreateAircraftUseCase
{
    private readonly IAircraftRepository _repository;
    private readonly IAircraftModelRepository _modelRepository;
    private readonly IAirlineRepository _airlineRepository;

    public CreateAircraftUseCase(
        IAircraftRepository repository,
        IAircraftModelRepository modelRepository,
        IAirlineRepository airlineRepository)
    {
        _repository       = repository;
        _modelRepository  = modelRepository;
        _airlineRepository = airlineRepository;
    }

    public async Task<AircraftEntity> ExecuteAsync(
        int modeloAvionId,
        int aerolineaId,
        string matricula,
        DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento)
    {
        _ = await _modelRepository.GetByIdAsync(modeloAvionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {modeloAvionId}.");

        var aerolinea = await _airlineRepository.GetByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        if (!aerolinea.Activa)
            throw new InvalidOperationException(
                $"La aerolínea '{aerolinea.Nombre}' está inactiva. " +
                "No se pueden registrar aviones para aerolíneas inactivas.");

        var matriculaVO = MatriculaAircraft.Crear(matricula);

        if (await _repository.ExistsByMatriculaAsync(matriculaVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un avión con la matrícula '{matriculaVO.Valor}'.");

        var entity = new AircraftEntity
        {
            ModeloAvionId             = modeloAvionId,
            AerolineaId               = aerolineaId,
            Matricula                 = matriculaVO.Valor,
            FechaFabricacion          = fechaFabricacion is not null
                ? FechaFabricacionAircraft.Crear(fechaFabricacion.Value).Valor
                : null,
            FechaProximoMantenimiento = fechaProximoMantenimiento is not null
                ? FechaProximoMantenimientoAircraft
                    .Crear(fechaProximoMantenimiento.Value).Valor
                : null,
            TotalHorasVuelo = 0,
            Estado          = "DISPONIBLE",
            Activo          = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}