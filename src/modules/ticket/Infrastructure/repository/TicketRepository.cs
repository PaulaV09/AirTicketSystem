// src/modules/ticket/Infrastructure/repository/TicketRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.ticket.Infrastructure.repository;

public class TicketRepository : BaseRepository<TicketEntity>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context) { }

    public async Task<TicketEntity?> GetByCodigoTiqueteAsync(string codigoTiquete)
        => await _dbSet
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Reserva)
                    .ThenInclude(r => r.Vuelo)
            .Include(t => t.AsientoConfirmado)
            .FirstOrDefaultAsync(t =>
                t.CodigoTiquete == codigoTiquete.ToUpperInvariant());

    public async Task<TicketEntity?> GetByPasajeroReservaAsync(int pasajeroReservaId)
        => await _dbSet
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Include(t => t.AsientoConfirmado)
            .FirstOrDefaultAsync(t =>
                t.PasajeroReservaId == pasajeroReservaId);

    public async Task<IEnumerable<TicketEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(t => t.Estado == estado.ToUpperInvariant())
            .OrderByDescending(t => t.FechaEmision)
            .ToListAsync();

    public async Task<IEnumerable<TicketEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Include(t => t.PasajeroReserva)
                .ThenInclude(pr => pr.Reserva)
            .Include(t => t.AsientoConfirmado)
            .Where(t => t.PasajeroReserva.Reserva.VueloId == vueloId)
            .OrderBy(t => t.AsientoConfirmado!.Fila)
            .ThenBy(t => t.AsientoConfirmado!.Columna)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoTiqueteAsync(string codigoTiquete)
        => await _dbSet
            .AnyAsync(t =>
                t.CodigoTiquete == codigoTiquete.ToUpperInvariant());

    public async Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId)
        => await _dbSet
            .AnyAsync(t => t.PasajeroReservaId == pasajeroReservaId);
}