// src/modules/payment/Infrastructure/repository/PaymentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.payment.Infrastructure.entity;

namespace AirTicketSystem.modules.payment.Infrastructure.repository;

public class PaymentRepository : BaseRepository<PaymentEntity>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<PaymentEntity>> GetByReservaAsync(int reservaId)
        => await _dbSet
            .Include(p => p.MetodoPago)
            .Where(p => p.ReservaId == reservaId)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();

    public async Task<PaymentEntity?> GetAprobadoByReservaAsync(int reservaId)
        => await _dbSet
            .Include(p => p.MetodoPago)
            .FirstOrDefaultAsync(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO");

    public async Task<IEnumerable<PaymentEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Where(p => p.Estado == estado.ToUpperInvariant())
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();

    public async Task<IEnumerable<PaymentEntity>> GetVencidosAsync()
        => await _dbSet
            .Where(p =>
                p.Estado == "PENDIENTE" &&
                p.FechaVencimiento <= DateTime.UtcNow)
            .ToListAsync();

    public async Task<bool> ReservaTienePagoAprobadoAsync(int reservaId)
        => await _dbSet
            .AnyAsync(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO");

    public async Task<decimal> SumarPagosAprobadosByReservaAsync(int reservaId)
        => await _dbSet
            .Where(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO")
            .SumAsync(p => p.Monto);
}