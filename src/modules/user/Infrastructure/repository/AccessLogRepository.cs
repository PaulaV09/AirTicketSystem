// src/modules/user/Infrastructure/repository/AccessLogRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Infrastructure.repository;

public class AccessLogRepository
    : BaseRepository<AccessLogEntity>, IAccessLogRepository
{
    public AccessLogRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AccessLogEntity>> GetByUsuarioAsync(int usuarioId)
        => await _dbSet
            .Where(l => l.UsuarioId == usuarioId)
            .OrderByDescending(l => l.FechaAcceso)
            .ToListAsync();

    public async Task<IEnumerable<AccessLogEntity>> GetByUsuarioAndTipoAsync(
        int usuarioId, string tipo)
        => await _dbSet
            .Where(l =>
                l.UsuarioId == usuarioId &&
                l.Tipo == tipo.ToUpperInvariant())
            .OrderByDescending(l => l.FechaAcceso)
            .ToListAsync();

    public async Task<AccessLogEntity?> GetUltimoLoginAsync(int usuarioId)
        => await _dbSet
            .Where(l =>
                l.UsuarioId == usuarioId &&
                l.Tipo == "LOGIN")
            .OrderByDescending(l => l.FechaAcceso)
            .FirstOrDefaultAsync();

    public async Task<int> ContarIntentosFallidosRecientesAsync(
        int usuarioId, DateTime desde)
        => await _dbSet
            .CountAsync(l =>
                l.UsuarioId == usuarioId &&
                l.Tipo == "INTENTO_FALLIDO" &&
                l.FechaAcceso >= desde);
}