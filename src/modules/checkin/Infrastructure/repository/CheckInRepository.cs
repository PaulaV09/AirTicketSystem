// src/modules/checkin/Infrastructure/repository/CheckInRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.checkin.Infrastructure.entity;

namespace AirTicketSystem.modules.checkin.Infrastructure.repository;

public class CheckInRepository : BaseRepository<CheckInEntity>, ICheckInRepository
{
    public CheckInRepository(AppDbContext context) : base(context) { }

    public async Task<CheckInEntity?> GetByPasajeroReservaAsync(int pasajeroReservaId)
        => await _dbSet
            .Include(c => c.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Include(c => c.Trabajador)
                .ThenInclude(w => w!.Persona)
            .Include(c => c.PaseAbordar)
            .FirstOrDefaultAsync(c =>
                c.PasajeroReservaId == pasajeroReservaId);

    public async Task<IEnumerable<CheckInEntity>> GetByTrabajadorAsync(int trabajadorId)
        => await _dbSet
            .Include(c => c.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(c => c.TrabajadorId == trabajadorId)
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();

    public async Task<IEnumerable<CheckInEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Include(c => c.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(c => c.Estado == estado.ToUpperInvariant())
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();

    public async Task<IEnumerable<CheckInEntity>> GetByTipoAsync(string tipo)
        => await _dbSet
            .Include(c => c.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(c => c.Tipo == tipo.ToUpperInvariant())
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();

    public async Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId)
        => await _dbSet
            .AnyAsync(c => c.PasajeroReservaId == pasajeroReservaId);
}