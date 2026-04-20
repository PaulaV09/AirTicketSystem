// src/modules/airline/Application/UseCases/RemoveAirlinePhoneUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class RemoveAirlinePhoneUseCase
{
    private readonly IAirlinePhoneRepository _phoneRepository;

    public RemoveAirlinePhoneUseCase(IAirlinePhoneRepository phoneRepository)
    {
        _phoneRepository = phoneRepository;
    }

    public async Task ExecuteAsync(int phoneId)
    {
        var telefono = await _phoneRepository.GetByIdAsync(phoneId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el teléfono con ID {phoneId}.");

        if (telefono.EsPrincipal)
            throw new InvalidOperationException(
                "No se puede eliminar el teléfono principal. " +
                "Asigne otro teléfono como principal primero.");

        await _phoneRepository.DeleteAsync(phoneId);
    }
}