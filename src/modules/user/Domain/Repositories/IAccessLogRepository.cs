// src/modules/user/Domain/Repositories/IAccessLogRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Domain.Repositories;

public interface IAccessLogRepository : IRepository<AccessLogEntity>
{
    Task<IEnumerable<AccessLogEntity>> GetByUsuarioAsync(int usuarioId);
    Task<IEnumerable<AccessLogEntity>> GetByUsuarioAndTipoAsync(
        int usuarioId, string tipo);
    Task<AccessLogEntity?> GetUltimoLoginAsync(int usuarioId);
    Task<int> ContarIntentosFallidosRecientesAsync(
        int usuarioId, DateTime desde);
}