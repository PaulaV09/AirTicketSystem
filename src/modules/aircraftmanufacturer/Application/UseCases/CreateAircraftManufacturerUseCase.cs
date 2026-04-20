// src/modules/aircraftmanufacturer/Application/UseCases/CreateAircraftManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class CreateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturerRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateAircraftManufacturerUseCase(
        IAircraftManufacturerRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<AircraftManufacturerEntity> ExecuteAsync(
        int paisId, string nombre, string? sitioWeb)
    {
        _ = await _countryRepository.GetByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var nombreVO = NombreAircraftManufacturer.Crear(nombre);

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un fabricante con el nombre '{nombreVO.Valor}'.");

        var entity = new AircraftManufacturerEntity
        {
            PaisId   = paisId,
            Nombre   = nombreVO.Valor,
            SitioWeb = sitioWeb is not null
                ? SitioWebAircraftManufacturer.Crear(sitioWeb).Valor
                : null
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}