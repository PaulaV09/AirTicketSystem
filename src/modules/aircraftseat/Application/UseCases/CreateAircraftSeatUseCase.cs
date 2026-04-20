// src/modules/aircraftseat/Application/UseCases/CreateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class CreateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;
    private readonly IAircraftRepository _aircraftRepository;
    private readonly IServiceClassRepository _serviceClassRepository;

    public CreateAircraftSeatUseCase(
        IAircraftSeatRepository repository,
        IAircraftRepository aircraftRepository,
        IServiceClassRepository serviceClassRepository)
    {
        _repository             = repository;
        _aircraftRepository     = aircraftRepository;
        _serviceClassRepository = serviceClassRepository;
    }

    public async Task<AircraftSeatEntity> ExecuteAsync(
        int avionId,
        int claseServicioId,
        int fila,
        char columna,
        bool esVentana,
        bool esPasillo,
        decimal costoSeleccion)
    {
        var avion = await _aircraftRepository.GetByIdAsync(avionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {avionId}.");

        if (!avion.Activo)
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' está dado de baja. " +
                "No se pueden agregar asientos.");

        _ = await _serviceClassRepository.GetByIdAsync(claseServicioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {claseServicioId}.");

        if (esVentana && esPasillo)
            throw new InvalidOperationException(
                "Un asiento no puede ser de ventana y de pasillo al mismo tiempo.");

        var filaVO    = FilaAircraftSeat.Crear(fila);
        var columnaVO = ColumnaAircraftSeat.Crear(columna);
        var codigoVO  = CodigoAsientoAircraftSeat.Crear(fila, columna);
        var costoVO   = CostoSeleccionAircraftSeat.Crear(costoSeleccion);

        if (await _repository.ExistsByCodigoAndAvionAsync(codigoVO.Valor, avionId))
            throw new InvalidOperationException(
                $"Ya existe el asiento '{codigoVO.Valor}' en el avión " +
                $"'{avion.Matricula}'.");

        var entity = new AircraftSeatEntity
        {
            AvionId         = avionId,
            ClaseServicioId = claseServicioId,
            CodigoAsiento   = codigoVO.Valor,
            Fila            = filaVO.Valor,
            Columna         = columnaVO.Valor.ToString(),
            EsVentana       = esVentana,
            EsPasillo       = esPasillo,
            CostoSeleccion  = costoVO.Valor,
            Activo          = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}