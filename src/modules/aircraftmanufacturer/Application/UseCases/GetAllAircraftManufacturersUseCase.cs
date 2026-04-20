// src/modules/aircraftmanufacturer/Application/UseCases/GetAllAircraftManufacturersUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class GetAllAircraftManufacturersUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAllAircraftManufacturersUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AircraftManufacturerEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(f => f.Nombre);
}