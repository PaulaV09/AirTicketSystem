// src/modules/milescuenta/Application/Services/MilesCuentaService.cs
using AirTicketSystem.modules.milescuenta.Application.Interfaces;
using AirTicketSystem.modules.milescuenta.Application.UseCases;
using AirTicketSystem.modules.milescuenta.Domain.aggregate;

namespace AirTicketSystem.modules.milescuenta.Application.Services;

public sealed class MilesCuentaService : IMilesCuentaService
{
    private readonly CrearCuentaMilesUseCase _crear;
    private readonly GetCuentaMilesByClienteUseCase _getByCliente;
    private readonly GetAllCuentasMilesUseCase _getAll;
    private readonly GetSaldoMilesUseCase _getSaldo;

    public MilesCuentaService(
        CrearCuentaMilesUseCase crear,
        GetCuentaMilesByClienteUseCase getByCliente,
        GetAllCuentasMilesUseCase getAll,
        GetSaldoMilesUseCase getSaldo)
    {
        _crear        = crear;
        _getByCliente = getByCliente;
        _getAll       = getAll;
        _getSaldo     = getSaldo;
    }

    public Task<MilesCuenta> CrearCuentaAsync(int clienteId)
        => _crear.ExecuteAsync(clienteId);

    public Task<MilesCuenta> GetByClienteAsync(int clienteId)
        => _getByCliente.ExecuteAsync(clienteId);

    public Task<IReadOnlyCollection<MilesCuenta>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<int> GetSaldoAsync(int clienteId)
        => _getSaldo.ExecuteAsync(clienteId);
}
