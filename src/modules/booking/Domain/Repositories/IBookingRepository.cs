// src/modules/booking/Domain/Repositories/IBookingRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.booking.Infrastructure.entity;

namespace AirTicketSystem.modules.booking.Domain.Repositories;

public interface IBookingRepository : IRepository<BookingEntity>
{
    Task<BookingEntity?> GetByCodigoReservaAsync(string codigoReserva);
    Task<BookingEntity?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<BookingEntity>> GetByClienteAsync(int clienteId);
    Task<IEnumerable<BookingEntity>> GetByVueloAsync(int vueloId);
    Task<IEnumerable<BookingEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<BookingEntity>> GetExpirasAsync();
    Task<bool> ExistsByCodigoReservaAsync(string codigoReserva);
    Task<int> ContarReservasByVueloAsync(int vueloId);
}