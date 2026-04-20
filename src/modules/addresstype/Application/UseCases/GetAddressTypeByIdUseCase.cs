// src/modules/addresstype/Application/UseCases/GetAddressTypeByIdUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public class GetAddressTypeByIdUseCase
{
    private readonly IAddressTypeRepository _repository;

    public GetAddressTypeByIdUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<AddressTypeEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de dirección no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de dirección con ID {id}.");
    }
}