// src/modules/aircraftmodel/Application/UseCases/CreateAircraftModelUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class CreateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;
    private readonly IAircraftManufacturerRepository _manufacturerRepository;

    public CreateAircraftModelUseCase(
        IAircraftModelRepository repository,
        IAircraftManufacturerRepository manufacturerRepository)
    {
        _repository            = repository;
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<AircraftModelEntity> ExecuteAsync(
        int fabricanteId,
        string nombre,
        string codigoModelo,
        int? autonomiKm,
        int? velocidadKmh,
        string? descripcion)
    {
        _ = await _manufacturerRepository.GetByIdAsync(fabricanteId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {fabricanteId}.");

        var nombreVO  = NombreAircraftModel.Crear(nombre);
        var codigoVO  = CodigoModeloAircraftModel.Crear(codigoModelo);

        if (await _repository.ExistsByCodigoModeloAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un modelo con el código '{codigoVO.Valor}'.");

        var entity = new AircraftModelEntity
        {
            FabricanteId        = fabricanteId,
            Nombre              = nombreVO.Valor,
            CodigoModelo        = codigoVO.Valor,
            AutonomiKm          = autonomiKm is not null
                ? AutonomiKmAircraftModel.Crear(autonomiKm.Value).Valor
                : null,
            VelocidadCruceroKmh = velocidadKmh is not null
                ? VelocidadCruceroKmhAircraftModel.Crear(velocidadKmh.Value).Valor
                : null,
            Descripcion = descripcion is not null
                ? DescripcionAircraftModel.Crear(descripcion).Valor
                : null
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}