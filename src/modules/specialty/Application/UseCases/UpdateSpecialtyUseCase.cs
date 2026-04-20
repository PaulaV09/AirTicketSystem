// src/modules/specialty/Application/UseCases/UpdateSpecialtyUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;
using AirTicketSystem.modules.specialty.Domain.ValueObjects;
using AirTicketSystem.modules.workertype.Domain.Repositories;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class UpdateSpecialtyUseCase
{
    private readonly ISpecialtyRepository _repository;
    private readonly IWorkerTypeRepository _workerTypeRepository;

    public UpdateSpecialtyUseCase(
        ISpecialtyRepository repository,
        IWorkerTypeRepository workerTypeRepository)
    {
        _repository          = repository;
        _workerTypeRepository = workerTypeRepository;
    }

    public async Task<SpecialtyEntity> ExecuteAsync(
        int id, string nombre, int? tipoTrabajadorId)
    {
        var especialidad = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {id}.");

        var nombreVO = NombreSpecialty.Crear(nombre);

        if (nombreVO.Valor != especialidad.Nombre &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otra especialidad con el nombre '{nombreVO.Valor}'.");

        if (tipoTrabajadorId.HasValue)
        {
            _ = await _workerTypeRepository.GetByIdAsync(tipoTrabajadorId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró un tipo de trabajador con ID " +
                    $"{tipoTrabajadorId.Value}.");
        }

        especialidad.Nombre           = nombreVO.Valor;
        especialidad.TipoTrabajadorId = tipoTrabajadorId;

        await _repository.UpdateAsync(especialidad);
        return especialidad;
    }
}