// src/modules/department/Application/UseCases/GetDepartmentByIdUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Application.UseCases;

public class GetDepartmentByIdUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentByIdUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<DepartmentEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del departamento no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {id}.");
    }
}