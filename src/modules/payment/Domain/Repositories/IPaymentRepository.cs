// src/modules/payment/Domain/Repositories/IPaymentRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.payment.Infrastructure.entity;

namespace AirTicketSystem.modules.payment.Domain.Repositories;

public interface IPaymentRepository : IRepository<PaymentEntity>
{
    Task<IEnumerable<PaymentEntity>> GetByReservaAsync(int reservaId);
    Task<PaymentEntity?> GetAprobadoByReservaAsync(int reservaId);
    Task<IEnumerable<PaymentEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<PaymentEntity>> GetVencidosAsync();
    Task<bool> ReservaTienePagoAprobadoAsync(int reservaId);
    Task<decimal> SumarPagosAprobadosByReservaAsync(int reservaId);
}