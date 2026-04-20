// src/modules/worker/Application/UseCases/UpdateWorkerSalaryUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.worker.Domain.ValueObjects;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public class UpdateWorkerSalaryUseCase
{
    private readonly IWorkerRepository _repository;

    public UpdateWorkerSalaryUseCase(IWorkerRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerEntity> ExecuteAsync(int id, decimal nuevoSalario)
    {
        var trabajador = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (!trabajador.Activo)
            throw new InvalidOperationException(
                "No se puede actualizar el salario de un trabajador inactivo.");

        var nuevoSalarioVO = SalarioWorker.Crear(nuevoSalario);

        if (nuevoSalarioVO.Valor <= trabajador.Salario)
            throw new InvalidOperationException(
                $"El nuevo salario ({nuevoSalarioVO.Valor:N2}) debe ser mayor " +
                $"al salario actual ({trabajador.Salario:N2}). " +
                "Para reducciones salariales contacte al área de RRHH.");

        trabajador.Salario = nuevoSalarioVO.Valor;
        await _repository.UpdateAsync(trabajador);
        return trabajador;
    }
}