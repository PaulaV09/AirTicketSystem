// src/modules/luggage/Infrastructure/repository/LuggageRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.luggage.Domain.Repositories;
using AirTicketSystem.modules.luggage.Infrastructure.entity;

namespace AirTicketSystem.modules.luggage.Infrastructure.repository;

public class LuggageRepository : BaseRepository<LuggageEntity>, ILuggageRepository
{
    public LuggageRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<LuggageEntity>> GetByPasajeroReservaAsync(
        int pasajeroReservaId)
        => await _dbSet
            .Include(l => l.TipoEquipaje)
            .Where(l => l.PasajeroReservaId == pasajeroReservaId)
            .OrderBy(l => l.TipoEquipaje.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<LuggageEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Include(l => l.TipoEquipaje)
            .Include(l => l.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(l => l.VueloId == vueloId)
            .OrderBy(l => l.CodigoEquipaje)
            .ToListAsync();

    public async Task<LuggageEntity?> GetByCodigoEquipajeAsync(string codigoEquipaje)
        => await _dbSet
            .Include(l => l.TipoEquipaje)
            .Include(l => l.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .FirstOrDefaultAsync(l =>
                l.CodigoEquipaje == codigoEquipaje.ToUpperInvariant());

    public async Task<IEnumerable<LuggageEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Include(l => l.TipoEquipaje)
            .Include(l => l.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Where(l => l.Estado == estado.ToUpperInvariant())
            .ToListAsync();

    public async Task<IEnumerable<LuggageEntity>> GetConIncidenciasAsync()
        => await _dbSet
            .Include(l => l.TipoEquipaje)
            .Include(l => l.PasajeroReserva)
                .ThenInclude(pr => pr.Persona)
            .Include(l => l.PasajeroReserva)
                .ThenInclude(pr => pr.Reserva)
                    .ThenInclude(r => r.Vuelo)
            .Where(l =>
                l.Estado == "PERDIDO" || l.Estado == "DAÑADO")
            .OrderByDescending(l => l.VueloId)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoEquipajeAsync(string codigoEquipaje)
        => await _dbSet
            .AnyAsync(l =>
                l.CodigoEquipaje == codigoEquipaje.ToUpperInvariant());
}