// src/modules/pilotlicense/Application/UseCases/RenewPilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;
using AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class RenewPilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public RenewPilotLicenseUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<PilotLicenseEntity> ExecuteAsync(
        int id, DateOnly nuevaFechaVencimiento)
    {
        var licencia = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {id}.");

        var nuevaFechaVO = FechaVencimientoPilotLicense.Crear(nuevaFechaVencimiento);

        if (nuevaFechaVO.Valor <= licencia.FechaVencimiento)
            throw new InvalidOperationException(
                "La nueva fecha de vencimiento debe ser posterior " +
                "a la fecha de vencimiento actual.");

        licencia.FechaVencimiento = nuevaFechaVO.Valor;
        licencia.Activa           = true;

        await _repository.UpdateAsync(licencia);
        return licencia;
    }
}