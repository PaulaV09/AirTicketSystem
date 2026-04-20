// src/modules/pilotlicense/Application/UseCases/CreatePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;
using AirTicketSystem.modules.pilotlicense.Domain.ValueObjects;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public class CreatePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;
    private readonly IWorkerRepository _workerRepository;

    public CreatePilotLicenseUseCase(
        IPilotLicenseRepository repository,
        IWorkerRepository workerRepository)
    {
        _repository      = repository;
        _workerRepository = workerRepository;
    }

    public async Task<PilotLicenseEntity> ExecuteAsync(
        int trabajadorId,
        string numeroLicencia,
        string tipoLicencia,
        DateOnly fechaExpedicion,
        DateOnly fechaVencimiento,
        string autoridadEmisora)
    {
        var trabajador = await _workerRepository.GetByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!trabajador.Activo)
            throw new InvalidOperationException(
                "No se pueden crear licencias para un trabajador inactivo.");

        var numeroVO     = NumeroLicenciaPilotLicense.Crear(numeroLicencia);
        var tipoVO       = TipoLicenciaPilotLicense.Crear(tipoLicencia);
        var expedicionVO = FechaExpedicionPilotLicense.Crear(fechaExpedicion);
        var vencimientoVO = FechaVencimientoPilotLicense.Crear(fechaVencimiento);
        var autoridadVO  = AutoridadEmisoraPilotLicense.Crear(autoridadEmisora);

        if (vencimientoVO.Valor <= expedicionVO.Valor)
            throw new InvalidOperationException(
                "La fecha de vencimiento debe ser posterior a la fecha de expedición.");

        if (await _repository.ExistsByNumeroLicenciaAsync(numeroVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una licencia con el número '{numeroVO.Valor}'.");

        var entity = new PilotLicenseEntity
        {
            TrabajadorId     = trabajadorId,
            NumeroLicencia   = numeroVO.Valor,
            TipoLicencia     = tipoVO.Valor,
            FechaExpedicion  = expedicionVO.Valor,
            FechaVencimiento = vencimientoVO.Valor,
            AutoridadEmisora = autoridadVO.Valor,
            Activa           = true
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}