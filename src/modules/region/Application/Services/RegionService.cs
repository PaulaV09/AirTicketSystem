// src/modules/region/Application/Services/RegionService.cs
using AirTicketSystem.modules.region.Application.Interfaces;
using AirTicketSystem.modules.region.Application.UseCases;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Application.Services;

public class RegionService : IRegionService
{
    private readonly CreateRegionUseCase _create;
    private readonly GetRegionByIdUseCase _getById;
    private readonly GetAllRegionsUseCase _getAll;
    private readonly GetRegionsByCountryUseCase _getByCountry;
    private readonly UpdateRegionUseCase _update;
    private readonly DeleteRegionUseCase _delete;

    public RegionService(
        CreateRegionUseCase create,
        GetRegionByIdUseCase getById,
        GetAllRegionsUseCase getAll,
        GetRegionsByCountryUseCase getByCountry,
        UpdateRegionUseCase update,
        DeleteRegionUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByCountry = getByCountry;
        _update      = update;
        _delete      = delete;
    }

    public Task<RegionEntity> CreateAsync(int paisId, string nombre, string? codigo)
        => _create.ExecuteAsync(paisId, nombre, codigo);

    public Task<RegionEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<RegionEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<RegionEntity>> GetByCountryAsync(int paisId)
        => _getByCountry.ExecuteAsync(paisId);

    public Task<RegionEntity> UpdateAsync(int id, string nombre, string? codigo)
        => _update.ExecuteAsync(id, nombre, codigo);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}