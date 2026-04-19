// src/modules/bookinghistory/Infrastructure/repository/BookingHistoryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Infrastructure.entity;

namespace AirTicketSystem.modules.bookinghistory.Infrastructure.repository;

public class BookingHistoryRepository
    : BaseRepository<BookingHistoryEntity>, IBookingHistoryRepository
{
    public BookingHistoryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<BookingHistoryEntity>> GetByReservaAsync(int reservaId)
        => await _dbSet
            .Where(h => h.ReservaId == reservaId)
            .OrderByDescending(h => h.FechaCambio)
            .ToListAsync();

    public async Task<BookingHistoryEntity?> GetUltimoCambioByReservaAsync(int reservaId)
        => await _dbSet
            .Where(h => h.ReservaId == reservaId)
            .OrderByDescending(h => h.FechaCambio)
            .FirstOrDefaultAsync();
}