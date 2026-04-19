// src/modules/bookinghistory/Domain/Repositories/IBookingHistoryRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.bookinghistory.Infrastructure.entity;

namespace AirTicketSystem.modules.bookinghistory.Domain.Repositories;

public interface IBookingHistoryRepository : IRepository<BookingHistoryEntity>
{
    Task<IEnumerable<BookingHistoryEntity>> GetByReservaAsync(int reservaId);
    Task<BookingHistoryEntity?> GetUltimoCambioByReservaAsync(int reservaId);
}