// src/modules/department/Application/Services/DepartmentService.cs
using AirTicketSystem.modules.department.Application.Interfaces;
using AirTicketSystem.modules.department.Application.UseCases;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly CreateDepartmentUseCase _create;
    private readonly GetDepartmentByIdUseCase _getById;
    private readonly GetAllDepartmentsUseCase _getAll;
    private readonly GetDepartmentsByRegionUseCase _getByCountry;
    private readonly UpdateDepartmentUseCase _update;
    private readonly DeleteDepartmentUseCase _delete;

    public DepartmentService(
        CreateDepartmentUseCase create,
        GetDepartmentByIdUseCase getById,
        GetAllDepartmentsUseCase getAll,
        GetDepartmentsByRegionUseCase getByCountry,
        UpdateDepartmentUseCase update,
        DeleteDepartmentUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByCountry = getByCountry;
        _update      = update;
        _delete      = delete;
    }

    public Task<DepartmentEntity> CreateAsync(int paisId, string nombre, string? codigo)
        => _create.ExecuteAsync(paisId, nombre, codigo);

    public Task<DepartmentEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<DepartmentEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<DepartmentEntity>> GetByCountryAsync(int paisId)
        => _getByCountry.ExecuteAsync(paisId);

    public Task<DepartmentEntity> UpdateAsync(int id, string nombre, string? codigo)
        => _update.ExecuteAsync(id, nombre, codigo);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}