// src/modules/phonetype/Application/Services/PhoneTypeService.cs
using AirTicketSystem.modules.phonetype.Application.Interfaces;
using AirTicketSystem.modules.phonetype.Application.UseCases;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Application.Services;

public class PhoneTypeService : IPhoneTypeService
{
    private readonly CreatePhoneTypeUseCase _create;
    private readonly GetPhoneTypeByIdUseCase _getById;
    private readonly GetAllPhoneTypesUseCase _getAll;
    private readonly UpdatePhoneTypeUseCase _update;
    private readonly DeletePhoneTypeUseCase _delete;

    public PhoneTypeService(
        CreatePhoneTypeUseCase create,
        GetPhoneTypeByIdUseCase getById,
        GetAllPhoneTypesUseCase getAll,
        UpdatePhoneTypeUseCase update,
        DeletePhoneTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<PhoneTypeEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<PhoneTypeEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<PhoneTypeEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<PhoneTypeEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}