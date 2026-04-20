// src/modules/serviceclass/Application/UseCases/GetAllServiceClassesUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class GetAllServiceClassesUseCase
{
    private readonly IServiceClassRepository _repository;

    public GetAllServiceClassesUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ServiceClassEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(s => s.Nombre);
}