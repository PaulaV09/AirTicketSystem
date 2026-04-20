// src/modules/airline/Application/UseCases/GetAirlineByIataUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetAirlineByIataUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAirlineByIataUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<AirlineEntity> ExecuteAsync(string codigoIata)
    {
        var iataVO = CodigoIataAerolinea.Crear(codigoIata);

        return await _repository.GetByCodigoIataAsync(iataVO.Valor)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con código IATA '{iataVO.Valor}'.");
    }
}