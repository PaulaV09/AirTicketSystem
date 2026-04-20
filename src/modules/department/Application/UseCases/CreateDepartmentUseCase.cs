// src/modules/department/Application/UseCases/CreateDepartmentUseCase.cs
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;
using AirTicketSystem.modules.department.Domain.ValueObjects;
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public class CreateDepartmentUseCase
{
    private readonly IDepartmentRepository _repository;
    private readonly IRegionRepository _regionRepository;

    public CreateDepartmentUseCase(
        IDepartmentRepository repository,
        IRegionRepository regionRepository)
    {
        _repository        = repository;
        _regionRepository = regionRepository;
    }

    public async Task<DepartmentEntity> ExecuteAsync(
        int regionId, string nombre, string? codigo)
    {
        _ = await _regionRepository.GetByIdAsync(regionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {regionId}.");

        var nombreVO = NombreDepartment.Crear(nombre);

        if (await _repository.ExistsByNombreAndRegionAsync(nombreVO.Valor, regionId))
            throw new InvalidOperationException(
                $"Ya existe un departamento con el nombre '{nombreVO.Valor}' " +
                $"en la región con ID {regionId}.");

        string? codigoNormalizado = null;
        if (codigo is not null)
            codigoNormalizado = CodigoDepartment.Crear(codigo).Valor;

        var entity = new DepartmentEntity
        {
            RegionId = regionId,
            Nombre = nombreVO.Valor,
            Codigo = codigoNormalizado
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}