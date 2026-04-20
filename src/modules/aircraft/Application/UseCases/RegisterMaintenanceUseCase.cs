// src/modules/aircraft/Application/UseCases/RegisterMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class RegisterMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public RegisterMaintenanceUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id, DateOnly fechaProximoMantenimiento)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        if (avion.Estado != "MANTENIMIENTO")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' no está en mantenimiento. " +
                $"Estado actual: '{avion.Estado}'.");

        var hoy      = DateOnly.FromDateTime(DateTime.Today);
        var proximaVO = FechaProximoMantenimientoAircraft
            .Crear(fechaProximoMantenimiento);

        avion.Estado                     = "DISPONIBLE";
        avion.FechaUltimoMantenimiento   = hoy;
        avion.FechaProximoMantenimiento  = proximaVO.Valor;

        await _repository.UpdateAsync(avion);
    }
}