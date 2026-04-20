// src/modules/city/Application/UseCases/CreateCityUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.city.Domain.ValueObjects;
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class CreateCityUseCase
{
    private readonly ICityRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;

    public CreateCityUseCase(
        ICityRepository repository,
        IDepartmentRepository departmentRepository)
    {
        _repository        = repository;
        _departmentRepository = departmentRepository;
    }

    public async Task<CityEntity> ExecuteAsync(
        int departmentId, string nombre, string? codigo)
    {
        _ = await _departmentRepository.GetByIdAsync(departmentId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {departmentId}.");

        var nombreVO = NombreCity.Crear(nombre);

        if (await _repository.ExistsByNombreAndDepartamentoAsync(nombreVO.Valor, departmentId))
            throw new InvalidOperationException(
                $"Ya existe una ciudad con el nombre '{nombreVO.Valor}' " +
                $"en el departamento con ID {departmentId}.");

        string? codigoNormalizado = null;
        if (codigo is not null)
            codigoNormalizado = CodigoPostalCity.Crear(codigo).Valor;

        var entity = new CityEntity
        {
            DepartamentoId = departmentId,
            Nombre = nombreVO.Valor,
            CodigoPostal = codigoNormalizado
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}