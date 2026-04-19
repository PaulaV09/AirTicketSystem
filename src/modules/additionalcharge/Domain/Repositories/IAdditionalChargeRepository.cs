// src/modules/additionalcharge/Domain/Repositories/IAdditionalChargeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.additionalcharge.Infrastructure.entity;

namespace AirTicketSystem.modules.additionalcharge.Domain.Repositories;

public interface IAdditionalChargeRepository : IRepository<AdditionalChargeEntity>
{
    Task<IEnumerable<AdditionalChargeEntity>> GetByReservaAsync(int reservaId);
    Task<decimal> SumarCargosByReservaAsync(int reservaId);
}