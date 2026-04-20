// src/modules/aircraftseat/Application/UseCases/UpdateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class UpdateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public UpdateAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftSeatEntity> ExecuteAsync(
        int id, bool esVentana, bool esPasillo, decimal costoSeleccion)
    {
        var asiento = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        if (esVentana && esPasillo)
            throw new InvalidOperationException(
                "Un asiento no puede ser de ventana y de pasillo al mismo tiempo.");

        asiento.EsVentana      = esVentana;
        asiento.EsPasillo      = esPasillo;
        asiento.CostoSeleccion = CostoSeleccionAircraftSeat.Crear(costoSeleccion).Valor;

        await _repository.UpdateAsync(asiento);
        return asiento;
    }
}