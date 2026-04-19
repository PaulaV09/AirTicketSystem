// src/modules/specialty/Domain/Repositories/ISpecialtyRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Domain.Repositories;

public interface ISpecialtyRepository : IRepository<SpecialtyEntity>
{
    Task<IEnumerable<SpecialtyEntity>> GetByTipoTrabajadorAsync(int tipoTrabajadorId);
    Task<IEnumerable<SpecialtyEntity>> GetGeneralesAsync();
    Task<bool> ExistsByNombreAsync(string nombre);
}