// src/modules/aircraftmanufacturer/Application/UseCases/UpdateAircraftManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class UpdateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public UpdateAircraftManufacturerUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftManufacturerEntity> ExecuteAsync(
        int id, string nombre, string? sitioWeb)
    {
        var fabricante = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {id}.");

        var nombreVO = NombreAircraftManufacturer.Crear(nombre);

        if (nombreVO.Valor != fabricante.Nombre &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro fabricante con el nombre '{nombreVO.Valor}'.");

        fabricante.Nombre   = nombreVO.Valor;
        fabricante.SitioWeb = sitioWeb is not null
            ? SitioWebAircraftManufacturer.Crear(sitioWeb).Valor
            : null;

        await _repository.UpdateAsync(fabricante);
        return fabricante;
    }
}