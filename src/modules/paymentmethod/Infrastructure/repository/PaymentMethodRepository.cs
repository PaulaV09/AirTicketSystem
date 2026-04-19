// src/modules/paymentmethod/Infrastructure/repository/PaymentMethodRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

namespace AirTicketSystem.modules.paymentmethod.Infrastructure.repository;

public class PaymentMethodRepository
    : BaseRepository<PaymentMethodEntity>, IPaymentMethodRepository
{
    public PaymentMethodRepository(AppDbContext context) : base(context) { }

    public async Task<PaymentMethodEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .FirstOrDefaultAsync(pm =>
                pm.Nombre.ToLower() == nombre.ToLower());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(pm => pm.Nombre.ToLower() == nombre.ToLower());
}