// src/modules/gender/Application/UseCases/GetAllGendersUseCase.cs
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public class GetAllGendersUseCase
{
    private readonly IGenderRepository _repository;

    public GetAllGendersUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GenderEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(g => g.Nombre);
}