// src/modules/department/Application/UseCases/UpdateDepartmentUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;
using AirTicketSystem.modules.department.Domain.ValueObjects;

namespace AirTicketSystem.modules.department.Application.UseCases;

public class UpdateDepartmentUseCase
{
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<DepartmentEntity> ExecuteAsync(
        int id, string nombre, string? codigo)
    {
        var department = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {id}.");

        var nombreVO = NombreDepartment.Crear(nombre);

        if (nombreVO.Valor != department.Nombre &&
            await _repository.ExistsByNombreAndRegionAsync(nombreVO.Valor, department.RegionId))
            throw new InvalidOperationException(
                $"Ya existe otro departamento con el nombre '{nombreVO.Valor}' " +
                $"en el mismo país.");

        department.Nombre = nombreVO.Valor;
        department.Codigo = codigo is not null
            ? CodigoDepartment.Crear(codigo).Valor
            : null;

        await _repository.UpdateAsync(department);
        return department;
    }
}