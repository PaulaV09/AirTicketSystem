// src/modules/milescuenta/Domain/Repositories/IMilesCuentaRepository.cs
using AirTicketSystem.modules.milescuenta.Domain.aggregate;

namespace AirTicketSystem.modules.milescuenta.Domain.Repositories;

public interface IMilesCuentaRepository
{
    Task<MilesCuenta?> FindByIdAsync(int id);
    Task<MilesCuenta?> FindByClienteAsync(int clienteId);
    Task<IReadOnlyCollection<MilesCuenta>> FindAllAsync();
    Task<bool> ExistsByClienteAsync(int clienteId);
    Task SaveAsync(MilesCuenta cuenta);
    Task UpdateAsync(MilesCuenta cuenta);
}
