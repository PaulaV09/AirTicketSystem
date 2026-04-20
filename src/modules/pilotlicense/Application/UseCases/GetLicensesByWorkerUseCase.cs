// src/modules/pilotlicense/Application/UseCases/GetLicensesByWorkerUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class GetLicensesByWorkerUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetLicensesByWorkerUseCase(IPilotLicenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PilotLicenseEntity>> ExecuteAsync(int trabajadorId)
    {
        if (trabajadorId <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        return await _repository.GetByTrabajadorAsync(trabajadorId);
    }
}