// src/modules/workertype/Application/UseCases/UpdateWorkerTypeUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Infrastructure.entity;
using AirTicketSystem.modules.workertype.Domain.ValueObjects;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class UpdateWorkerTypeUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public UpdateWorkerTypeUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerTypeEntity> ExecuteAsync(int id, string nombre)
    {
        var tipo = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {id}.");

        var nombreVO = NombreWorkerType.Crear(nombre);

        if (nombreVO.Valor != tipo.Nombre &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de trabajador con el nombre '{nombreVO.Valor}'.");

        tipo.Nombre = nombreVO.Valor;
        await _repository.UpdateAsync(tipo);
        return tipo;
    }
}