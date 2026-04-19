// src/modules/invoice/Domain/Repositories/IInvoiceRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.invoice.Infrastructure.entity;

namespace AirTicketSystem.modules.invoice.Domain.Repositories;

public interface IInvoiceRepository : IRepository<InvoiceEntity>
{
    Task<InvoiceEntity?> GetByReservaAsync(int reservaId);
    Task<InvoiceEntity?> GetByNumeroFacturaAsync(string numeroFactura);
    Task<IEnumerable<InvoiceEntity>> GetByFechaRangoAsync(
        DateTime desde, DateTime hasta);
    Task<bool> ExistsByReservaAsync(int reservaId);
    Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura);
}