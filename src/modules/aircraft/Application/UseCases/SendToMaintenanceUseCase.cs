// src/modules/aircraft/Application/UseCases/SendToMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class SendToMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public SendToMaintenanceUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id, DateOnly fechaProximoMantenimiento)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        if (avion.Estado == "EN_VUELO")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' está en vuelo. " +
                "No se puede enviar a mantenimiento.");

        if (avion.Estado == "MANTENIMIENTO")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' ya está en mantenimiento.");

        if (avion.Estado == "FUERA_DE_SERVICIO")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' está fuera de servicio.");

        var fechaVO = FechaProximoMantenimientoAircraft
            .Crear(fechaProximoMantenimiento);

        avion.Estado                    = "MANTENIMIENTO";
        avion.FechaProximoMantenimiento = fechaVO.Valor;

        await _repository.UpdateAsync(avion);
    }
}