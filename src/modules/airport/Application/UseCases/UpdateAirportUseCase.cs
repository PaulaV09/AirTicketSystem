// src/modules/airport/Application/UseCases/UpdateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.airport.Domain.ValueObjects;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class UpdateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public UpdateAirportUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<AirportEntity> ExecuteAsync(
        int id, string nombre, string? direccion)
    {
        var aeropuerto = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");

        aeropuerto.Nombre    = NombreAirport.Crear(nombre).Valor;
        aeropuerto.Direccion = direccion is not null
            ? DireccionAirport.Crear(direccion).Valor
            : null;

        await _repository.UpdateAsync(aeropuerto);
        return aeropuerto;
    }
}