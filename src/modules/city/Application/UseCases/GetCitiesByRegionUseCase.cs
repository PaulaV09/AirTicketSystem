// src/modules/city/Application/UseCases/GetCitiesByRegionUseCase.cs
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Application.UseCases;

public class GetCitiesByDepartmentUseCase
{
    private readonly ICityRepository _repository;

    public GetCitiesByDepartmentUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CityEntity>> ExecuteAsync(int departmentId)
    {
        if (departmentId <= 0)
            throw new ArgumentException("El ID del departamento no es válido.");

        return await _repository.GetByDepartamentoAsync(departmentId);
    }
}