// src/modules/aircraftmodel/Infrastructure/repository/AircraftModelRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Infrastructure.repository;

public class AircraftModelRepository
    : BaseRepository<AircraftModelEntity>, IAircraftModelRepository
{
    public AircraftModelRepository(AppDbContext context) : base(context) { }

    public async Task<AircraftModelEntity?> GetByCodigoModeloAsync(string codigoModelo)
        => await _dbSet
            .Include(m => m.Fabricante)
            .FirstOrDefaultAsync(m =>
                m.CodigoModelo == codigoModelo.ToUpperInvariant());

    public async Task<IEnumerable<AircraftModelEntity>> GetByFabricanteAsync(int fabricanteId)
        => await _dbSet
            .Include(m => m.Fabricante)
            .Where(m => m.FabricanteId == fabricanteId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoModeloAsync(string codigoModelo)
        => await _dbSet
            .AnyAsync(m => m.CodigoModelo == codigoModelo.ToUpperInvariant());
}