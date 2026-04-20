// src/modules/route/Application/UseCases/ActivateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class ActivateRouteUseCase
{
    private readonly IRouteRepository _repository;

    public ActivateRouteUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var ruta = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {id}.");

        if (ruta.Activa)
            throw new InvalidOperationException(
                "La ruta ya se encuentra activa.");

        ruta.Activa = true;
        await _repository.UpdateAsync(ruta);
    }
}