// src/modules/milescuenta/Application/UseCases/GetCuentaMilesByClienteUseCase.cs
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milescuenta.Application.UseCases;

public sealed class GetCuentaMilesByClienteUseCase
{
    private readonly IMilesCuentaRepository _repository;

    public GetCuentaMilesByClienteUseCase(IMilesCuentaRepository repository)
        => _repository = repository;

    public async Task<MilesCuenta> ExecuteAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        return await _repository.FindByClienteAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"El cliente {clienteId} no tiene una cuenta de millas registrada.");
    }
}
