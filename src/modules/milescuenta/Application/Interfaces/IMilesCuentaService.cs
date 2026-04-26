// src/modules/milescuenta/Application/Interfaces/IMilesCuentaService.cs
using AirTicketSystem.modules.milescuenta.Domain.aggregate;

namespace AirTicketSystem.modules.milescuenta.Application.Interfaces;

public interface IMilesCuentaService
{
    Task<MilesCuenta> CrearCuentaAsync(int clienteId);
    Task<MilesCuenta> GetByClienteAsync(int clienteId);
    Task<IReadOnlyCollection<MilesCuenta>> GetAllAsync();
    Task<int> GetSaldoAsync(int clienteId);
}
