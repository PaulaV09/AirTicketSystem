// src/modules/gender/Application/UseCases/GetGenderByIdUseCase.cs
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Application.UseCases;

public class GetGenderByIdUseCase
{
    private readonly IGenderRepository _repository;

    public GetGenderByIdUseCase(IGenderRepository repository)
    {
        _repository = repository;
    }

    public async Task<GenderEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del género no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un género con ID {id}.");
    }
}