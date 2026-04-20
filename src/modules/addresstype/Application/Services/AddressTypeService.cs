// src/modules/addresstype/Application/Services/AddressTypeService.cs
using AirTicketSystem.modules.addresstype.Application.Interfaces;
using AirTicketSystem.modules.addresstype.Application.UseCases;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Application.Services;

public class AddressTypeService : IAddressTypeService
{
    private readonly CreateAddressTypeUseCase _create;
    private readonly GetAddressTypeByIdUseCase _getById;
    private readonly GetAllAddressTypesUseCase _getAll;
    private readonly UpdateAddressTypeUseCase _update;
    private readonly DeleteAddressTypeUseCase _delete;

    public AddressTypeService(
        CreateAddressTypeUseCase create,
        GetAddressTypeByIdUseCase getById,
        GetAllAddressTypesUseCase getAll,
        UpdateAddressTypeUseCase update,
        DeleteAddressTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<AddressTypeEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<AddressTypeEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<AddressTypeEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<AddressTypeEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}