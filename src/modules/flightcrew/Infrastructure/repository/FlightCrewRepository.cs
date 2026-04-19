// src/modules/flightcrew/Infrastructure/repository/FlightCrewRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;

namespace AirTicketSystem.modules.flightcrew.Infrastructure.repository;

public class FlightCrewRepository
    : BaseRepository<FlightCrewEntity>, IFlightCrewRepository
{
    public FlightCrewRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<FlightCrewEntity>> GetByVueloAsync(int vueloId)
        => await _dbSet
            .Include(fc => fc.Trabajador)
                .ThenInclude(w => w.Persona)
            .Where(fc => fc.VueloId == vueloId)
            .OrderBy(fc => fc.RolEnVuelo)
            .ToListAsync();

    public async Task<IEnumerable<FlightCrewEntity>> GetByTrabajadorAsync(
        int trabajadorId)
        => await _dbSet
            .Include(fc => fc.Vuelo)
            .Where(fc => fc.TrabajadorId == trabajadorId)
            .OrderByDescending(fc => fc.Vuelo.FechaSalida)
            .ToListAsync();

    public async Task<FlightCrewEntity?> GetByVueloAndRolAsync(
        int vueloId, string rol)
        => await _dbSet
            .Include(fc => fc.Trabajador)
                .ThenInclude(w => w.Persona)
            .FirstOrDefaultAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == rol.ToUpperInvariant());

    public async Task<bool> VueloTienePilotoAsync(int vueloId)
        => await _dbSet
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == "PILOTO");

    public async Task<bool> VueloTieneCopiloAsync(int vueloId)
        => await _dbSet
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == "COPILOTO");

    public async Task<bool> ExistsByVueloAndTrabajadorAsync(
        int vueloId, int trabajadorId)
        => await _dbSet
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.TrabajadorId == trabajadorId);

    public async Task<bool> ExistsByVueloAndRolAsync(int vueloId, string rol)
        => await _dbSet
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == rol.ToUpperInvariant());
}