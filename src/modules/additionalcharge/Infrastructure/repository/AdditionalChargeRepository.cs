// src/modules/additionalcharge/Infrastructure/repository/AdditionalChargeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;
using AirTicketSystem.modules.additionalcharge.Infrastructure.entity;

namespace AirTicketSystem.modules.additionalcharge.Infrastructure.repository;

public class AdditionalChargeRepository
    : BaseRepository<AdditionalChargeEntity>, IAdditionalChargeRepository
{
    public AdditionalChargeRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<AdditionalChargeEntity>> GetByReservaAsync(int reservaId)
        => await _dbSet
            .Where(ac => ac.ReservaId == reservaId)
            .OrderByDescending(ac => ac.Fecha)
            .ToListAsync();

    public async Task<decimal> SumarCargosByReservaAsync(int reservaId)
        => await _dbSet
            .Where(ac => ac.ReservaId == reservaId)
            .SumAsync(ac => ac.Monto);
}