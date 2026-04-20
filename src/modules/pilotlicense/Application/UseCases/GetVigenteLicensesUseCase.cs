// src/modules/pilotlicense/Application/UseCases/GetVigenteLicensesUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class GetVigenteLicensesUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetVigenteLicensesUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PilotLicenseEntity>> ExecuteAsync()
        => await _repository.GetVigentesAsync();
}