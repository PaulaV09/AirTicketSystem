// src/modules/route/Application/UseCases/DeleteRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class DeleteRouteUseCase
{
    private readonly IRouteRepository _repository;

    public DeleteRouteUseCase(IRouteRepository repository)
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
                "No se puede eliminar una ruta activa. " +
                "Desactívela primero.");

        await _repository.DeleteAsync(id);
    }
}