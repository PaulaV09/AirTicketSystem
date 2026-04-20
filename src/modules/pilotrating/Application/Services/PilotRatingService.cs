// src/modules/pilotrating/Application/Services/PilotRatingService.cs
using AirTicketSystem.modules.pilotrating.Application.Interfaces;
using AirTicketSystem.modules.pilotrating.Application.UseCases;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.Services;

public class PilotRatingService : IPilotRatingService
{
    private readonly CreatePilotRatingUseCase _create;
    private readonly GetPilotRatingByIdUseCase _getById;
    private readonly GetRatingsByLicenseUseCase _getByLicense;
    private readonly GetRatingsByAircraftModelUseCase _getByModel;
    private readonly GetVigenteRatingsUseCase _getVigentes;
    private readonly RenewPilotRatingUseCase _renew;
    private readonly DeletePilotRatingUseCase _delete;

    public PilotRatingService(
        CreatePilotRatingUseCase create,
        GetPilotRatingByIdUseCase getById,
        GetRatingsByLicenseUseCase getByLicense,
        GetRatingsByAircraftModelUseCase getByModel,
        GetVigenteRatingsUseCase getVigentes,
        RenewPilotRatingUseCase renew,
        DeletePilotRatingUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getByLicense = getByLicense;
        _getByModel  = getByModel;
        _getVigentes = getVigentes;
        _renew       = renew;
        _delete      = delete;
    }

    public Task<PilotRatingEntity> CreateAsync(
        int licenciaId, int modeloAvionId,
        DateOnly fechaHabilitacion, DateOnly fechaVencimiento)
        => _create.ExecuteAsync(
            licenciaId, modeloAvionId, fechaHabilitacion, fechaVencimiento);

    public Task<PilotRatingEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<PilotRatingEntity>> GetByLicenseAsync(int licenciaId)
        => _getByLicense.ExecuteAsync(licenciaId);

    public Task<IEnumerable<PilotRatingEntity>> GetByAircraftModelAsync(int modeloAvionId)
        => _getByModel.ExecuteAsync(modeloAvionId);

    public Task<IEnumerable<PilotRatingEntity>> GetVigentesAsync()
        => _getVigentes.ExecuteAsync();

    public Task<PilotRatingEntity> RenewAsync(int id, DateOnly nuevaFechaVencimiento)
        => _renew.ExecuteAsync(id, nuevaFechaVencimiento);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}