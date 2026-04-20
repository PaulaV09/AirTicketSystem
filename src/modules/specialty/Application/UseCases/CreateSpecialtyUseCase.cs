// src/modules/specialty/Application/UseCases/CreateSpecialtyUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;
using AirTicketSystem.modules.specialty.Domain.ValueObjects;
using AirTicketSystem.modules.workertype.Domain.Repositories;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class CreateSpecialtyUseCase
{
    private readonly ISpecialtyRepository _repository;
    private readonly IWorkerTypeRepository _workerTypeRepository;

    public CreateSpecialtyUseCase(
        ISpecialtyRepository repository,
        IWorkerTypeRepository workerTypeRepository)
    {
        _repository          = repository;
        _workerTypeRepository = workerTypeRepository;
    }

    public async Task<SpecialtyEntity> ExecuteAsync(
        string nombre, int? tipoTrabajadorId)
    {
        var nombreVO = NombreSpecialty.Crear(nombre);

        if (tipoTrabajadorId.HasValue)
        {
            _ = await _workerTypeRepository.GetByIdAsync(tipoTrabajadorId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró un tipo de trabajador con ID " +
                    $"{tipoTrabajadorId.Value}.");
        }

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una especialidad con el nombre '{nombreVO.Valor}'.");

        var entity = new SpecialtyEntity
        {
            Nombre            = nombreVO.Valor,
            TipoTrabajadorId  = tipoTrabajadorId
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}