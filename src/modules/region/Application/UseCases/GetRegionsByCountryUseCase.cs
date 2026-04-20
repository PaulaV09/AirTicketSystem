// src/modules/region/Application/UseCases/GetRegionsByCountryUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class GetRegionsByCountryUseCase
{
    private readonly IRegionRepository _repository;

    public GetRegionsByCountryUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RegionEntity>> ExecuteAsync(int paisId)
    {
        if (paisId <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        return await _repository.GetByPaisAsync(paisId);
    }
}