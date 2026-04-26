// src/modules/milescuenta/Application/UseCases/GetSaldoMilesUseCase.cs
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milescuenta.Application.UseCases;

public sealed class GetSaldoMilesUseCase
{
    private readonly IMilesCuentaRepository _repository;

    public GetSaldoMilesUseCase(IMilesCuentaRepository repository)
        => _repository = repository;

    public async Task<int> ExecuteAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var cuenta = await _repository.FindByClienteAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"El cliente {clienteId} no tiene una cuenta de millas registrada.");

        return cuenta.SaldoActual.Valor;
    }
}
