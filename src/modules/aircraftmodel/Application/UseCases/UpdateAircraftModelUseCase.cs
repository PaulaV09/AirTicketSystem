// src/modules/aircraftmodel/Application/UseCases/UpdateAircraftModelUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class UpdateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;

    public UpdateAircraftModelUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftModelEntity> ExecuteAsync(
        int id,
        string nombre,
        int? autonomiKm,
        int? velocidadKmh,
        string? descripcion)
    {
        var modelo = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {id}.");

        modelo.Nombre = NombreAircraftModel.Crear(nombre).Valor;

        modelo.AutonomiKm = autonomiKm is not null
            ? AutonomiKmAircraftModel.Crear(autonomiKm.Value).Valor
            : null;

        modelo.VelocidadCruceroKmh = velocidadKmh is not null
            ? VelocidadCruceroKmhAircraftModel.Crear(velocidadKmh.Value).Valor
            : null;

        modelo.Descripcion = descripcion is not null
            ? DescripcionAircraftModel.Crear(descripcion).Valor
            : null;

        await _repository.UpdateAsync(modelo);
        return modelo;
    }
}