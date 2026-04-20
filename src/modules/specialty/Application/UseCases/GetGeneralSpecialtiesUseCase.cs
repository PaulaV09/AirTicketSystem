// src/modules/specialty/Application/UseCases/GetGeneralSpecialtiesUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetGeneralSpecialtiesUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetGeneralSpecialtiesUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SpecialtyEntity>> ExecuteAsync()
        => await _repository.GetGeneralesAsync();
}