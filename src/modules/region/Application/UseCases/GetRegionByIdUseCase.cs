// src/modules/region/Application/UseCases/GetRegionByIdUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class GetRegionByIdUseCase
{
    private readonly IRegionRepository _repository;

    public GetRegionByIdUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegionEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la región no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {id}.");
    }
}