// src/modules/invoice/Infrastructure/repository/InvoiceRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.invoice.Domain.Repositories;
using AirTicketSystem.modules.invoice.Infrastructure.entity;

namespace AirTicketSystem.modules.invoice.Infrastructure.repository;

public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext context) : base(context) { }

    public async Task<InvoiceEntity?> GetByReservaAsync(int reservaId)
        => await _dbSet
            .Include(i => i.Reserva)
            .Include(i => i.DireccionFacturacion)
                .ThenInclude(d => d.Ciudad)
            .FirstOrDefaultAsync(i => i.ReservaId == reservaId);

    public async Task<InvoiceEntity?> GetByNumeroFacturaAsync(string numeroFactura)
        => await _dbSet
            .Include(i => i.Reserva)
                .ThenInclude(r => r.Cliente)
                    .ThenInclude(c => c.Persona)
            .Include(i => i.DireccionFacturacion)
            .FirstOrDefaultAsync(i =>
                i.NumeroFactura == numeroFactura.ToUpperInvariant());

    public async Task<IEnumerable<InvoiceEntity>> GetByFechaRangoAsync(
        DateTime desde, DateTime hasta)
        => await _dbSet
            .Include(i => i.Reserva)
                .ThenInclude(r => r.Cliente)
                    .ThenInclude(c => c.Persona)
            .Where(i =>
                i.FechaEmision >= desde &&
                i.FechaEmision <= hasta)
            .OrderByDescending(i => i.FechaEmision)
            .ToListAsync();

    public async Task<bool> ExistsByReservaAsync(int reservaId)
        => await _dbSet
            .AnyAsync(i => i.ReservaId == reservaId);

    public async Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura)
        => await _dbSet
            .AnyAsync(i =>
                i.NumeroFactura == numeroFactura.ToUpperInvariant());
}