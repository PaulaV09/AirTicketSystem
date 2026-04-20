// src/modules/region/Application/UseCases/CreateRegionUseCase.cs
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;
using AirTicketSystem.modules.region.Domain.ValueObjects;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.region.Application.UseCases;

public class CreateRegionUseCase
{
    private readonly IRegionRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateRegionUseCase(
        IRegionRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<RegionEntity> ExecuteAsync(
        int paisId, string nombre, string? codigo)
    {
        _ = await _countryRepository.GetByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var nombreVO = NombreRegion.Crear(nombre);

        if (await _repository.ExistsByNombreAndPaisAsync(nombreVO.Valor, paisId))
            throw new InvalidOperationException(
                $"Ya existe una región con el nombre '{nombreVO.Valor}' " +
                $"en el país con ID {paisId}.");

        string? codigoNormalizado = null;
        if (codigo is not null)
            codigoNormalizado = CodigoRegion.Crear(codigo).Valor;

        var entity = new RegionEntity
        {
            PaisId = paisId,
            Nombre = nombreVO.Valor,
            Codigo = codigoNormalizado
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}