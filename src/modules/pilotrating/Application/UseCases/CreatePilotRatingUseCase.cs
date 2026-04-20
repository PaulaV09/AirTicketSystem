// src/modules/pilotrating/Application/UseCases/CreatePilotRatingUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;
using AirTicketSystem.modules.pilotrating.Domain.ValueObjects;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class CreatePilotRatingUseCase
{
    private readonly IPilotRatingRepository _repository;
    private readonly IPilotLicenseRepository _licenseRepository;
    private readonly IAircraftModelRepository _modelRepository;

    public CreatePilotRatingUseCase(
        IPilotRatingRepository repository,
        IPilotLicenseRepository licenseRepository,
        IAircraftModelRepository modelRepository)
    {
        _repository      = repository;
        _licenseRepository = licenseRepository;
        _modelRepository = modelRepository;
    }

    public async Task<PilotRatingEntity> ExecuteAsync(
        int licenciaId,
        int modeloAvionId,
        DateOnly fechaHabilitacion,
        DateOnly fechaVencimiento)
    {
        var licencia = await _licenseRepository.GetByIdAsync(licenciaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {licenciaId}.");

        if (!licencia.Activa)
            throw new InvalidOperationException(
                "No se pueden crear habilitaciones sobre una licencia " +
                "suspendida o inactiva.");

        var hoy = DateOnly.FromDateTime(DateTime.Today);
        if (licencia.FechaVencimiento < hoy)
            throw new InvalidOperationException(
                "No se pueden crear habilitaciones sobre una licencia vencida. " +
                "Renuévela primero.");

        _ = await _modelRepository.GetByIdAsync(modeloAvionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {modeloAvionId}.");

        var habilitacionVO  = FechaHabilitacionPilotRating.Crear(fechaHabilitacion);
        var vencimientoVO   = FechaVencimientoPilotRating.Crear(fechaVencimiento);

        if (vencimientoVO.Valor <= habilitacionVO.Valor)
            throw new InvalidOperationException(
                "La fecha de vencimiento debe ser posterior " +
                "a la fecha de habilitación.");

        if (await _repository.ExistsByLicenciaAndModeloAsync(licenciaId, modeloAvionId))
            throw new InvalidOperationException(
                "Ya existe una habilitación para esta licencia " +
                "y modelo de avión.");

        var entity = new PilotRatingEntity
        {
            LicenciaId        = licenciaId,
            ModeloAvionId     = modeloAvionId,
            FechaHabilitacion = habilitacionVO.Valor,
            FechaVencimiento  = vencimientoVO.Valor
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}