// src/modules/booking/Infrastructure/repository/BookingRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.booking.Infrastructure.entity;

namespace AirTicketSystem.modules.booking.Infrastructure.repository;

public class BookingRepository : BaseRepository<BookingEntity>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    public async Task<BookingEntity?> GetByCodigoReservaAsync(string codigoReserva)
        => await _dbSet
            .Include(b => b.Cliente)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Origen)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Destino)
            .Include(b => b.Tarifa)
            .FirstOrDefaultAsync(b =>
                b.CodigoReserva == codigoReserva.ToUpperInvariant());

    public async Task<BookingEntity?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(b => b.Cliente)
                .ThenInclude(c => c.Persona)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Origen)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Destino)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Aerolinea)
            .Include(b => b.Tarifa)
            .Include(b => b.Pasajeros)
                .ThenInclude(p => p.Persona)
            .Include(b => b.Pagos)
            .Include(b => b.Factura)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<BookingEntity>> GetByClienteAsync(int clienteId)
        => await _dbSet
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Origen)
            .Include(b => b.Vuelo)
                .ThenInclude(v => v.Ruta)
                    .ThenInclude(r => r.Destino)
            .Where(b => b.ClienteId == clienteId)
            .OrderByDescending(b => b.FechaReserva)
            .ToListAsync();

    public async Task<IEnumerable<BookingEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Include(b => b.Cliente)
                .ThenInclude(c => c.Persona)
            .Where(b => b.VueloId == vueloId)
            .OrderBy(b => b.FechaReserva)
            .ToListAsync();

    public async Task<IEnumerable<BookingEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Where(b => b.Estado == estado.ToUpperInvariant())
            .OrderByDescending(b => b.FechaReserva)
            .ToListAsync();

    public async Task<IEnumerable<BookingEntity>> GetExpirasAsync()
        => await _dbSet
            .Where(b =>
                b.Estado == "PENDIENTE" &&
                b.FechaExpiracion <= DateTime.UtcNow)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoReservaAsync(string codigoReserva)
        => await _dbSet
            .AnyAsync(b => b.CodigoReserva == codigoReserva.ToUpperInvariant());

    public async Task<int> ContarReservasByVueloAsync(int vueloId)
        => await _dbSet
            .CountAsync(b =>
                b.VueloId == vueloId &&
                b.Estado != "CANCELADA" &&
                b.Estado != "EXPIRADA");
}