// src/modules/flight/Application/UseCases/RegisterLandingFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.milesmovimiento.Application.UseCases;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class RegisterLandingFlightUseCase
{
    private readonly IFlightRepository             _flightRepository;
    private readonly IAircraftRepository           _aircraftRepository;
    private readonly AcumularMilesPorVueloUseCase  _acumularMiles;

    public RegisterLandingFlightUseCase(
        IFlightRepository            flightRepository,
        IAircraftRepository          aircraftRepository,
        AcumularMilesPorVueloUseCase acumularMiles)
    {
        _flightRepository   = flightRepository;
        _aircraftRepository = aircraftRepository;
        _acumularMiles      = acumularMiles;
    }

    public async Task ExecuteAsync(
        int id,
        DateTime fechaLlegadaReal,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        var avion = await _aircraftRepository.FindByIdAsync(flight.AvionId)
            ?? throw new KeyNotFoundException(
                "No se encontró el avión asociado al vuelo.");

        var duracionHoras = (decimal)(fechaLlegadaReal - flight.FechaSalida.Valor)
            .TotalHours;

        // 1. Ambos aggregates manejan su propio estado
        flight.RegistrarAterrizaje(fechaLlegadaReal);
        avion.RegistrarAterrizaje(duracionHoras);

        await _flightRepository.UpdateAsync(flight);
        await _aircraftRepository.UpdateAsync(avion);

        // 2. Acumulación automática de millas para todos los pasajeros
        //    con reserva CONFIRMADA en este vuelo.
        //    Si falla (ej. sin reservas), el aterrizaje ya quedó registrado.
        try
        {
            await _acumularMiles.ExecuteAsync(id, cancellationToken);
        }
        catch
        {
            // La acumulación es un proceso secundario: no revierte el aterrizaje.
            // El admin puede reejecutarla manualmente desde el menú de millas.
        }
    }
}