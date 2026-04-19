// src/modules/paymentmethod/Domain/Repositories/IPaymentMethodRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

namespace AirTicketSystem.modules.paymentmethod.Domain.Repositories;

public interface IPaymentMethodRepository : IRepository<PaymentMethodEntity>
{
    Task<PaymentMethodEntity?> GetByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
}