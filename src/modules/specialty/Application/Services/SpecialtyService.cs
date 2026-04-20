// src/modules/specialty/Application/Services/SpecialtyService.cs
using AirTicketSystem.modules.specialty.Application.Interfaces;
using AirTicketSystem.modules.specialty.Application.UseCases;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Application.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly CreateSpecialtyUseCase _create;
    private readonly GetSpecialtyByIdUseCase _getById;
    private readonly GetAllSpecialtiesUseCase _getAll;
    private readonly GetSpecialtiesByWorkerTypeUseCase _getByWorkerType;
    private readonly GetGeneralSpecialtiesUseCase _getGenerales;
    private readonly UpdateSpecialtyUseCase _update;
    private readonly DeleteSpecialtyUseCase _delete;

    public SpecialtyService(
        CreateSpecialtyUseCase create,
        GetSpecialtyByIdUseCase getById,
        GetAllSpecialtiesUseCase getAll,
        GetSpecialtiesByWorkerTypeUseCase getByWorkerType,
        GetGeneralSpecialtiesUseCase getGenerales,
        UpdateSpecialtyUseCase update,
        DeleteSpecialtyUseCase delete)
    {
        _create         = create;
        _getById        = getById;
        _getAll         = getAll;
        _getByWorkerType = getByWorkerType;
        _getGenerales   = getGenerales;
        _update         = update;
        _delete         = delete;
    }

    public Task<SpecialtyEntity> CreateAsync(string nombre, int? tipoTrabajadorId)
        => _create.ExecuteAsync(nombre, tipoTrabajadorId);

    public Task<SpecialtyEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<SpecialtyEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<SpecialtyEntity>> GetByWorkerTypeAsync(int tipoTrabajadorId)
        => _getByWorkerType.ExecuteAsync(tipoTrabajadorId);

    public Task<IEnumerable<SpecialtyEntity>> GetGeneralesAsync()
        => _getGenerales.ExecuteAsync();

    public Task<SpecialtyEntity> UpdateAsync(
        int id, string nombre, int? tipoTrabajadorId)
        => _update.ExecuteAsync(id, nombre, tipoTrabajadorId);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}