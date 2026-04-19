// src/modules/luggagetype/Domain/Repositories/ILuggageTypeRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagetype.Domain.Repositories;

public interface ILuggageTypeRepository : IRepository<LuggageTypeEntity>
{
    Task<LuggageTypeEntity?> GetByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
}