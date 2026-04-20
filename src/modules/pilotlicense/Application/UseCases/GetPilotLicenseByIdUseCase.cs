// src/modules/pilotlicense/Application/UseCases/GetPilotLicenseByIdUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class GetPilotLicenseByIdUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetPilotLicenseByIdUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<PilotLicenseEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");
    }
}