// src/modules/city/Application/Services/CityService.cs
using AirTicketSystem.modules.city.Application.Interfaces;
using AirTicketSystem.modules.city.Application.UseCases;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Application.Services;

public class CityService : ICityService
{
    private readonly CreateCityUseCase _create;
    private readonly GetCityByIdUseCase _getById;
    private readonly GetAllCitiesUseCase _getAll;
    private readonly GetCitiesByDepartmentUseCase _getByDepartment;
    private readonly UpdateCityUseCase _update;
    private readonly DeleteCityUseCase _delete;

    public CityService(
        CreateCityUseCase create,
        GetCityByIdUseCase getById,
        GetAllCitiesUseCase getAll,
        GetCitiesByDepartmentUseCase getByDepartment,
        UpdateCityUseCase update,
        DeleteCityUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByDepartment = getByDepartment;
        _update      = update;
        _delete      = delete;
    }

    public Task<CityEntity> CreateAsync(int departmentId, string nombre, string? codigo)
        => _create.ExecuteAsync(departmentId, nombre, codigo);

    public Task<CityEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<CityEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<CityEntity>> GetByDepartmentAsync(int departmentId)
        => _getByDepartment.ExecuteAsync(departmentId);

    public Task<CityEntity> UpdateAsync(int id, string nombre, string? codigo)
        => _update.ExecuteAsync(id, nombre, codigo);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}