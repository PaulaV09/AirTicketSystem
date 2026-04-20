// src/modules/specialty/Application/UseCases/GetAllSpecialtiesUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetAllSpecialtiesUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetAllSpecialtiesUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SpecialtyEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(s => s.Nombre);
}