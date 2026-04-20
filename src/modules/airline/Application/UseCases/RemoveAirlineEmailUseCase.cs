// src/modules/airline/Application/UseCases/RemoveAirlineEmailUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class RemoveAirlineEmailUseCase
{
    private readonly IAirlineEmailRepository _emailRepository;

    public RemoveAirlineEmailUseCase(IAirlineEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
    }

    public async Task ExecuteAsync(int emailId)
    {
        var email = await _emailRepository.GetByIdAsync(emailId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el email con ID {emailId}.");

        if (email.EsPrincipal)
            throw new InvalidOperationException(
                "No se puede eliminar el email principal. " +
                "Asigne otro email como principal primero.");

        await _emailRepository.DeleteAsync(emailId);
    }
}