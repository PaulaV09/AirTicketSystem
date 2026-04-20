// src/modules/region/Application/UseCases/UpdateRegionUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;
using AirTicketSystem.modules.region.Domain.ValueObjects;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class UpdateRegionUseCase
{
    private readonly IRegionRepository _repository;

    public UpdateRegionUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<RegionEntity> ExecuteAsync(
        int id, string nombre, string? codigo)
    {
        var region = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {id}.");

        var nombreVO = NombreRegion.Crear(nombre);

        if (nombreVO.Valor != region.Nombre &&
            await _repository.ExistsByNombreAndPaisAsync(nombreVO.Valor, region.PaisId))
            throw new InvalidOperationException(
                $"Ya existe otra región con el nombre '{nombreVO.Valor}' " +
                $"en el mismo país.");

        region.Nombre = nombreVO.Valor;
        region.Codigo = codigo is not null
            ? CodigoRegion.Crear(codigo).Valor
            : null;

        await _repository.UpdateAsync(region);
        return region;
    }
}