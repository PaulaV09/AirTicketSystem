// src/modules/pilotlicense/Application/UseCases/DeletePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class DeletePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public DeletePilotLicenseUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var licencia = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        if (licencia.Activa)
            throw new InvalidOperationException(
                "No se puede eliminar una licencia activa. " +
                "Suspéndala primero.");

        await _repository.DeleteAsync(id);
    }
}