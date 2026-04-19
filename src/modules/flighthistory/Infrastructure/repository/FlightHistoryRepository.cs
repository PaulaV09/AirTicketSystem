// src/modules/flighthistory/Infrastructure/repository/FlightHistoryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flighthistory.Domain.Repositories;
using AirTicketSystem.modules.flighthistory.Infrastructure.entity;

namespace AirTicketSystem.modules.flighthistory.Infrastructure.repository;

public class FlightHistoryRepository
    : BaseRepository<FlightHistoryEntity>, IFlightHistoryRepository
{
    public FlightHistoryRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<FlightHistoryEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Where(h => h.VueloId == vueloId)
            .OrderByDescending(h => h.FechaCambio)
            .ToListAsync();

    public async Task<FlightHistoryEntity?> GetUltimoCambioByVueloAsync(int vueloId)
        => await _dbSet
            .Where(h => h.VueloId == vueloId)
            .OrderByDescending(h => h.FechaCambio)
            .FirstOrDefaultAsync();
}