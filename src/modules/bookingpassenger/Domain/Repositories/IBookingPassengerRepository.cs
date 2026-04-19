// src/modules/bookingpassenger/Domain/Repositories/IBookingPassengerRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

namespace AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

public interface IBookingPassengerRepository : IRepository<BookingPassengerEntity>
{
    Task<IEnumerable<BookingPassengerEntity>> GetByReservaAsync(int reservaId);
    Task<BookingPassengerEntity?> GetByReservaAndPersonaAsync(
        int reservaId, int personaId);
    Task<IEnumerable<BookingPassengerEntity>> GetByPersonaAsync(int personaId);
    Task<bool> ExistsByReservaAndPersonaAsync(int reservaId, int personaId);
    Task<int> ContarPasajerosByReservaAsync(int reservaId);
}