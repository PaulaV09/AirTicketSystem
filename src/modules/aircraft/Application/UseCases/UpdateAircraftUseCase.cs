// src/modules/aircraft/Application/UseCases/UpdateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class UpdateAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public UpdateAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftEntity> ExecuteAsync(
        int id,
        DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        avion.FechaFabricacion = fechaFabricacion is not null
            ? FechaFabricacionAircraft.Crear(fechaFabricacion.Value).Valor
            : null;

        avion.FechaProximoMantenimiento = fechaProximoMantenimiento is not null
            ? FechaProximoMantenimientoAircraft
                .Crear(fechaProximoMantenimiento.Value).Valor
            : null;

        await _repository.UpdateAsync(avion);
        return avion;
    }
}