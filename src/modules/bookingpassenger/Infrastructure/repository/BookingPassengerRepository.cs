// src/modules/bookingpassenger/Infrastructure/repository/BookingPassengerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

namespace AirTicketSystem.modules.bookingpassenger.Infrastructure.repository;

public class BookingPassengerRepository
    : BaseRepository<BookingPassengerEntity>, IBookingPassengerRepository
{
    public BookingPassengerRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<BookingPassengerEntity>> GetByReservaAsync(int reservaId)
        => await _dbSet
            .Include(bp => bp.Persona)
            .Include(bp => bp.Asiento)
                .ThenInclude(a => a!.Asiento)
            .Where(bp => bp.ReservaId == reservaId)
            .ToListAsync();

    public async Task<BookingPassengerEntity?> GetByReservaAndPersonaAsync(
        int reservaId, int personaId)
        => await _dbSet
            .Include(bp => bp.Persona)
            .FirstOrDefaultAsync(bp =>
                bp.ReservaId == reservaId &&
                bp.PersonaId == personaId);

    public async Task<IEnumerable<BookingPassengerEntity>> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(bp => bp.Reserva)
                .ThenInclude(r => r.Vuelo)
                    .ThenInclude(v => v.Ruta)
            .Where(bp => bp.PersonaId == personaId)
            .OrderByDescending(bp => bp.Reserva.FechaReserva)
            .ToListAsync();

    public async Task<bool> ExistsByReservaAndPersonaAsync(int reservaId, int personaId)
        => await _dbSet
            .AnyAsync(bp =>
                bp.ReservaId == reservaId &&
                bp.PersonaId == personaId);

    public async Task<int> ContarPasajerosByReservaAsync(int reservaId)
        => await _dbSet
            .CountAsync(bp => bp.ReservaId == reservaId);
}