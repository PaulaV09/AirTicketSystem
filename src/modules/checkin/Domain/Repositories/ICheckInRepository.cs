// src/modules/checkin/Domain/Repositories/ICheckInRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.checkin.Infrastructure.entity;

namespace AirTicketSystem.modules.checkin.Domain.Repositories;

public interface ICheckInRepository : IRepository<CheckInEntity>
{
    Task<CheckInEntity?> GetByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IEnumerable<CheckInEntity>> GetByTrabajadorAsync(int trabajadorId);
    Task<IEnumerable<CheckInEntity>> GetByEstadoAsync(string estado);
    Task<IEnumerable<CheckInEntity>> GetByTipoAsync(string tipo);
    Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId);
}