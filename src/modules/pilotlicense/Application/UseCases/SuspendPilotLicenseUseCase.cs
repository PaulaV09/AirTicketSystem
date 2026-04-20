// src/modules/pilotlicense/Application/UseCases/SuspendPilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class SuspendPilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public SuspendPilotLicenseUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var licencia = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        if (!licencia.Activa)
            throw new InvalidOperationException(
                "La licencia ya se encuentra suspendida.");

        licencia.Activa = false;
        await _repository.UpdateAsync(licencia);
    }
}