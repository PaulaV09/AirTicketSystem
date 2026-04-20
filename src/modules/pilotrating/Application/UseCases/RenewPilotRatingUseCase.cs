// src/modules/pilotrating/Application/UseCases/RenewPilotRatingUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;
using AirTicketSystem.modules.pilotrating.Domain.ValueObjects;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class RenewPilotRatingUseCase
{
    private readonly IPilotRatingRepository _repository;

    public RenewPilotRatingUseCase(IPilotRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<PilotRatingEntity> ExecuteAsync(
        int id, DateOnly nuevaFechaVencimiento)
    {
        var habilitacion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una habilitación con ID {id}.");

        var nuevaFechaVO = FechaVencimientoPilotRating.Crear(nuevaFechaVencimiento);

        if (nuevaFechaVO.Valor <= habilitacion.FechaVencimiento)
            throw new InvalidOperationException(
                "La nueva fecha de vencimiento debe ser posterior " +
                "a la fecha actual de vencimiento.");

        habilitacion.FechaVencimiento = nuevaFechaVO.Valor;
        await _repository.UpdateAsync(habilitacion);
        return habilitacion;
    }
}