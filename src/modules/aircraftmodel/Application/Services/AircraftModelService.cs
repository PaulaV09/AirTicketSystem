// src/modules/aircraftmodel/Application/Services/AircraftModelService.cs
using AirTicketSystem.modules.aircraftmodel.Application.Interfaces;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Application.Services;

public class AircraftModelService : IAircraftModelService
{
    private readonly CreateAircraftModelUseCase _create;
    private readonly GetAircraftModelByIdUseCase _getById;
    private readonly GetAllAircraftModelsUseCase _getAll;
    private readonly GetAircraftModelsByManufacturerUseCase _getByManufacturer;
    private readonly UpdateAircraftModelUseCase _update;
    private readonly DeleteAircraftModelUseCase _delete;

    public AircraftModelService(
        CreateAircraftModelUseCase create,
        GetAircraftModelByIdUseCase getById,
        GetAllAircraftModelsUseCase getAll,
        GetAircraftModelsByManufacturerUseCase getByManufacturer,
        UpdateAircraftModelUseCase update,
        DeleteAircraftModelUseCase delete)
    {
        _create          = create;
        _getById         = getById;
        _getAll          = getAll;
        _getByManufacturer = getByManufacturer;
        _update          = update;
        _delete          = delete;
    }

    public Task<AircraftModelEntity> CreateAsync(
        int fabricanteId, string nombre, string codigoModelo,
        int? autonomiKm, int? velocidadKmh, string? descripcion)
        => _create.ExecuteAsync(
            fabricanteId, nombre, codigoModelo, autonomiKm, velocidadKmh, descripcion);

    public Task<AircraftModelEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<AircraftModelEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<AircraftModelEntity>> GetByManufacturerAsync(int fabricanteId)
        => _getByManufacturer.ExecuteAsync(fabricanteId);

    public Task<AircraftModelEntity> UpdateAsync(
        int id, string nombre, int? autonomiKm,
        int? velocidadKmh, string? descripcion)
        => _update.ExecuteAsync(id, nombre, autonomiKm, velocidadKmh, descripcion);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}