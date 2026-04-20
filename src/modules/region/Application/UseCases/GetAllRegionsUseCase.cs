// src/modules/region/Application/UseCases/GetAllRegionsUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class GetAllRegionsUseCase
{
    private readonly IRegionRepository _repository;

    public GetAllRegionsUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RegionEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(r => r.Nombre);
}