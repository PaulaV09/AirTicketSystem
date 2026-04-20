// src/modules/emailtype/Application/UseCases/GetAllEmailTypesUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public class GetAllEmailTypesUseCase
{
    private readonly IEmailTypeRepository _repository;

    public GetAllEmailTypesUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EmailTypeEntity>> ExecuteAsync()
        => (await _repository.GetAllAsync()).OrderBy(et => et.Descripcion);
}