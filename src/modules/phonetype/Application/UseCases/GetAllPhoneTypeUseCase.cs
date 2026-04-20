// src/modules/phonetype/Application/UseCases/GetAllPhoneTypesUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public class GetAllPhoneTypesUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public GetAllPhoneTypesUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PhoneTypeEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(pt => pt.Descripcion);
}