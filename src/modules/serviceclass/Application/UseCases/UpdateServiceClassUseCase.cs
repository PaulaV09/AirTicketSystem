// src/modules/serviceclass/Application/UseCases/UpdateServiceClassUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;
using AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class UpdateServiceClassUseCase
{
    private readonly IServiceClassRepository _repository;

    public UpdateServiceClassUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceClassEntity> ExecuteAsync(
        int id, string nombre, string? descripcion)
    {
        var clase = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {id}.");

        clase.Nombre      = NombreServiceClass.Crear(nombre).Valor;
        clase.Descripcion = descripcion is not null
            ? DescripcionServiceClass.Crear(descripcion).Valor
            : null;

        await _repository.UpdateAsync(clase);
        return clase;
    }
}