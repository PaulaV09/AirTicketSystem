// src/modules/airline/Application/UseCases/AddAirlineEmailUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.emailtype.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class AddAirlineEmailUseCase
{
    private readonly IAirlineRepository _airlineRepository;
    private readonly IAirlineEmailRepository _emailRepository;
    private readonly IEmailTypeRepository _emailTypeRepository;

    public AddAirlineEmailUseCase(
        IAirlineRepository airlineRepository,
        IAirlineEmailRepository emailRepository,
        IEmailTypeRepository emailTypeRepository)
    {
        _airlineRepository   = airlineRepository;
        _emailRepository     = emailRepository;
        _emailTypeRepository = emailTypeRepository;
    }

    public async Task<AirlineEmailEntity> ExecuteAsync(
        int aerolineaId,
        int tipoEmailId,
        string email,
        bool esPrincipal)
    {
        _ = await _airlineRepository.FindByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        _ = await _emailTypeRepository.GetByIdAsync(tipoEmailId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {tipoEmailId}.");

        var emailVO = EmailAirlineEmail.Crear(email);

        if (await _emailRepository.ExistsByEmailAndAerolineaAsync(
            emailVO.Valor, aerolineaId))
            throw new InvalidOperationException(
                $"El email '{emailVO.Valor}' ya está registrado " +
                "para esta aerolínea.");

        if (esPrincipal)
            await _emailRepository.DesmarcarPrincipalByAerolineaAsync(aerolineaId);

        var entity = new AirlineEmailEntity
        {
            AerolineaId = aerolineaId,
            TipoEmailId = tipoEmailId,
            Email       = emailVO.Valor,
            EsPrincipal = esPrincipal
        };

        await _emailRepository.AddAsync(entity);
        return entity;
    }
}