// src/modules/aircraftmanufacturer/Application/UseCases/GetAircraftManufacturerByIdUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class GetAircraftManufacturerByIdUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAircraftManufacturerByIdUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftManufacturerEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del fabricante no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {id}.");
    }
}