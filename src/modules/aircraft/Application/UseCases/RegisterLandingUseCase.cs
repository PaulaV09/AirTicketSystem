// src/modules/aircraft/Application/UseCases/RegisterLandingUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class RegisterLandingUseCase
{
    private readonly IAircraftRepository _repository;

    public RegisterLandingUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id, decimal horasVuelo)
    {
        var avion = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        if (avion.Estado != "EN_VUELO")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula}' no está en vuelo. " +
                "Solo se puede registrar aterrizaje de aviones en vuelo.");

        if (horasVuelo <= 0)
            throw new ArgumentException(
                "Las horas de vuelo deben ser mayores a 0.");

        var totalVO = TotalHorasVueloAircraft
            .Crear(avion.TotalHorasVuelo)
            .Sumar(horasVuelo);

        avion.TotalHorasVuelo = totalVO.Valor;
        avion.Estado          = "DISPONIBLE";

        await _repository.UpdateAsync(avion);
    }
}