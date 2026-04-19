// src/modules/boardingpass/Domain/Repositories/IBoardingPassRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;

namespace AirTicketSystem.modules.boardingpass.Domain.Repositories;

public interface IBoardingPassRepository : IRepository<BoardingPassEntity>
{
    Task<BoardingPassEntity?> GetByCodigoPaseAsync(string codigoPase);
    Task<BoardingPassEntity?> GetByCheckinAsync(int checkinId);
    Task<IEnumerable<BoardingPassEntity>> GetByPuertaAsync(int puertaEmbarqueId);
    Task<bool> ExistsByCodigoPaseAsync(string codigoPase);
    Task<bool> ExistsByCheckinAsync(int checkinId);
}