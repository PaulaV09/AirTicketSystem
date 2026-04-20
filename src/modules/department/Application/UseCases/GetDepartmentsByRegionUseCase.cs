// src/modules/department/Application/UseCases/GetDepartmentsByRegionUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.UseCases;

public class GetDepartmentsByRegionUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentsByRegionUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentEntity>> ExecuteAsync(int regionId)
    {
        if (regionId <= 0)
            throw new ArgumentException("El ID de la región no es válido.");

        return await _repository.GetByRegionAsync(regionId);
    }
}