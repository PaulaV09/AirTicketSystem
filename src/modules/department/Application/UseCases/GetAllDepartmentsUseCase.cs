// src/modules/department/Application/UseCases/GetAllDepartmentsUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.UseCases;

public class GetAllDepartmentsUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetAllDepartmentsUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(d => d.Nombre);
}