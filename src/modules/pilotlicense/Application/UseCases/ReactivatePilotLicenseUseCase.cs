// src/modules/pilotlicense/Application/UseCases/ReactivatePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class ReactivatePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public ReactivatePilotLicenseUseCase(IPilotLicenseRepository repository)
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
                "La licencia ya se encuentra activa.");

        var hoy = DateOnly.FromDateTime(DateTime.Today);
        if (licencia.FechaVencimiento < hoy)
            throw new InvalidOperationException(
                "No se puede reactivar una licencia vencida. " +
                "Debe renovarla primero antes de reactivarla.");

        licencia.Activa = true;
        await _repository.UpdateAsync(licencia);
    }
}