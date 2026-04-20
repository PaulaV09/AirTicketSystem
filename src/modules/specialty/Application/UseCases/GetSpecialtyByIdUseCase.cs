// src/modules/specialty/Application/UseCases/GetSpecialtyByIdUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetSpecialtyByIdUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetSpecialtyByIdUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<SpecialtyEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la especialidad no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {id}.");
    }
}