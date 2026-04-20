// src/modules/addresstype/Application/UseCases/GetAllAddressTypesUseCase.cs
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Application.UseCases;

public class GetAllAddressTypesUseCase
{
    private readonly IAddressTypeRepository _repository;

    public GetAllAddressTypesUseCase(IAddressTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AddressTypeEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(et => et.Descripcion);
}