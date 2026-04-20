// src/modules/phonetype/Application/UseCases/GetPhoneTypeByIdUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public class GetPhoneTypeByIdUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public GetPhoneTypeByIdUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneTypeEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de teléfono no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {id}.");
    }
}