// src/modules/airline/Application/UseCases/AddAirlinePhoneUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.phonetype.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class AddAirlinePhoneUseCase
{
    private readonly IAirlineRepository _airlineRepository;
    private readonly IAirlinePhoneRepository _phoneRepository;
    private readonly IPhoneTypeRepository _phoneTypeRepository;

    public AddAirlinePhoneUseCase(
        IAirlineRepository airlineRepository,
        IAirlinePhoneRepository phoneRepository,
        IPhoneTypeRepository phoneTypeRepository)
    {
        _airlineRepository   = airlineRepository;
        _phoneRepository     = phoneRepository;
        _phoneTypeRepository = phoneTypeRepository;
    }

    public async Task<AirlinePhoneEntity> ExecuteAsync(
        int aerolineaId,
        int tipoTelefonoId,
        string numero,
        string? indicativo,
        bool esPrincipal)
    {
        _ = await _airlineRepository.GetByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        _ = await _phoneTypeRepository.GetByIdAsync(tipoTelefonoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {tipoTelefonoId}.");

        var numeroVO = NumeroAirlinePhone.Crear(numero);

        if (await _phoneRepository.ExistsByNumeroAndAerolineaAsync(
            numeroVO.Valor, aerolineaId))
            throw new InvalidOperationException(
                $"El número '{numeroVO.Valor}' ya está registrado " +
                "para esta aerolínea.");

        // Si se marca como principal, desmarcar el anterior
        if (esPrincipal)
            await _phoneRepository.DesmarcarPrincipalByAerolineaAsync(aerolineaId);

        var entity = new AirlinePhoneEntity
        {
            AerolineaId    = aerolineaId,
            TipoTelefonoId = tipoTelefonoId,
            Numero         = numeroVO.Valor,
            IndicativoPais = indicativo is not null
                ? IndicativoPaisAirlinePhone.Crear(indicativo).Valor
                : null,
            EsPrincipal = esPrincipal
        };

        await _phoneRepository.AddAsync(entity);
        return entity;
    }
}