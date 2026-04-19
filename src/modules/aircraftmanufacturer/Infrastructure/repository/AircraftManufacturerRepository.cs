// src/modules/aircraftmanufacturer/Infrastructure/repository/AircraftManufacturerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.repository;

public class AircraftManufacturerRepository
    : BaseRepository<AircraftManufacturerEntity>, IAircraftManufacturerRepository
{
    public AircraftManufacturerRepository(AppDbContext context) : base(context) { }

    public async Task<AircraftManufacturerEntity?> GetByNombreAsync(string nombre)
        => await _dbSet
            .Include(f => f.Pais)
            .FirstOrDefaultAsync(f =>
                f.Nombre.ToLower() == nombre.ToLower());

    public async Task<IEnumerable<AircraftManufacturerEntity>> GetByPaisAsync(int paisId)
        => await _dbSet
            .Include(f => f.Pais)
            .Where(f => f.PaisId == paisId)
            .OrderBy(f => f.Nombre)
            .ToListAsync();

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _dbSet
            .AnyAsync(f => f.Nombre.ToLower() == nombre.ToLower());
}