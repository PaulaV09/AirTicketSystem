// src/modules/boardingpass/Infrastructure/repository/BoardingPassRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;

namespace AirTicketSystem.modules.boardingpass.Infrastructure.repository;

public class BoardingPassRepository
    : BaseRepository<BoardingPassEntity>, IBoardingPassRepository
{
    public BoardingPassRepository(AppDbContext context) : base(context) { }

    public async Task<BoardingPassEntity?> GetByCodigoPaseAsync(string codigoPase)
        => await _dbSet
            .Include(bp => bp.Checkin)
                .ThenInclude(c => c.PasajeroReserva)
                    .ThenInclude(pr => pr.Persona)
            .Include(bp => bp.PuertaEmbarque)
                .ThenInclude(g => g!.Terminal)
            .FirstOrDefaultAsync(bp =>
                bp.CodigoPase == codigoPase.ToUpperInvariant());

    public async Task<BoardingPassEntity?> GetByCheckinAsync(int checkinId)
        => await _dbSet
            .Include(bp => bp.PuertaEmbarque)
            .FirstOrDefaultAsync(bp => bp.CheckinId == checkinId);

    public async Task<IEnumerable<BoardingPassEntity>> GetByPuertaAsync(
        int puertaEmbarqueId)
        => await _dbSet
            .Include(bp => bp.Checkin)
                .ThenInclude(c => c.PasajeroReserva)
                    .ThenInclude(pr => pr.Persona)
            .Where(bp => bp.PuertaEmbarqueId == puertaEmbarqueId)
            .OrderBy(bp => bp.HoraEmbarque)
            .ToListAsync();

    public async Task<bool> ExistsByCodigoPaseAsync(string codigoPase)
        => await _dbSet
            .AnyAsync(bp =>
                bp.CodigoPase == codigoPase.ToUpperInvariant());

    public async Task<bool> ExistsByCheckinAsync(int checkinId)
        => await _dbSet
            .AnyAsync(bp => bp.CheckinId == checkinId);
}