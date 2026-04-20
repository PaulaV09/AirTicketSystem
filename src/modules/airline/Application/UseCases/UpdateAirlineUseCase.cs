// src/modules/airline/Application/UseCases/UpdateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class UpdateAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public UpdateAirlineUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<AirlineEntity> ExecuteAsync(
        int id,
        string nombre,
        string? nombreComercial,
        string? sitioWeb)
    {
        var aerolinea = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");

        aerolinea.Nombre          = NombreAerolinea.Crear(nombre).Valor;
        aerolinea.NombreComercial = nombreComercial is not null
            ? NombreComercialAerolinea.Crear(nombreComercial).Valor
            : null;
        aerolinea.SitioWeb = sitioWeb is not null
            ? SitioWebAerolinea.Crear(sitioWeb).Valor
            : null;

        await _repository.UpdateAsync(aerolinea);
        return aerolinea;
    }
}