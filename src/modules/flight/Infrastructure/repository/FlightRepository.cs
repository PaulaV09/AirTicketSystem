// src/modules/flight/Infrastructure/repository/FlightRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.flight.Infrastructure.repository;

public class FlightRepository : BaseRepository<FlightEntity>, IFlightRepository
{
    public FlightRepository(AppDbContext context) : base(context) { }

    public async Task<FlightEntity?> GetByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha)
        => await _dbSet
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(f => f.Avion)
            .FirstOrDefaultAsync(f =>
                f.NumeroVuelo == numeroVuelo.ToUpperInvariant() &&
                f.FechaSalida.Date == fecha.Date);

    public async Task<IEnumerable<FlightEntity>> GetByRutaAsync(int rutaId)
        => await _dbSet
            .Where(f => f.RutaId == rutaId)
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetByAvionAsync(int avionId)
        => await _dbSet
            .Where(f => f.AvionId == avionId)
            .OrderByDescending(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetByEstadoAsync(string estado)
        => await _dbSet
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Destino)
            .Where(f => f.Estado == estado.ToUpperInvariant())
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetByFechaAsync(DateTime fecha)
        => await _dbSet
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Where(f => f.FechaSalida.Date == fecha.Date)
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetByOrigenAndDestinoAsync(
        int origenId, int destinoId, DateTime fecha)
        => await _dbSet
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Destino)
            .Where(f =>
                f.Ruta.OrigenId == origenId &&
                f.Ruta.DestinoId == destinoId &&
                f.FechaSalida.Date == fecha.Date &&
                f.Estado == "PROGRAMADO")
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetProgramadosAsync()
        => await _dbSet
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(f => f.Ruta)
                .ThenInclude(r => r.Destino)
            .Where(f =>
                f.Estado == "PROGRAMADO" &&
                f.FechaSalida >= DateTime.UtcNow)
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<IEnumerable<FlightEntity>> GetConCheckinAbiertoAsync()
        => await _dbSet
            .Where(f =>
                f.CheckinApertura != null &&
                f.CheckinCierre != null &&
                f.CheckinApertura <= DateTime.UtcNow &&
                f.CheckinCierre >= DateTime.UtcNow)
            .OrderBy(f => f.FechaSalida)
            .ToListAsync();

    public async Task<bool> ExistsByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha)
        => await _dbSet
            .AnyAsync(f =>
                f.NumeroVuelo == numeroVuelo.ToUpperInvariant() &&
                f.FechaSalida.Date == fecha.Date);
}