// src/modules/serviceclass/Application/Services/ServiceClassService.cs
using AirTicketSystem.modules.serviceclass.Application.Interfaces;
using AirTicketSystem.modules.serviceclass.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Application.Services;

public class ServiceClassService : IServiceClassService
{
    private readonly CreateServiceClassUseCase _create;
    private readonly GetServiceClassByIdUseCase _getById;
    private readonly GetAllServiceClassesUseCase _getAll;
    private readonly UpdateServiceClassUseCase _update;
    private readonly DeleteServiceClassUseCase _delete;

    public ServiceClassService(
        CreateServiceClassUseCase create,
        GetServiceClassByIdUseCase getById,
        GetAllServiceClassesUseCase getAll,
        UpdateServiceClassUseCase update,
        DeleteServiceClassUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<ServiceClassEntity> CreateAsync(
        string nombre, string codigo, string? descripcion)
        => _create.ExecuteAsync(nombre, codigo, descripcion);

    public Task<ServiceClassEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<ServiceClassEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<ServiceClassEntity> UpdateAsync(
        int id, string nombre, string? descripcion)
        => _update.ExecuteAsync(id, nombre, descripcion);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}