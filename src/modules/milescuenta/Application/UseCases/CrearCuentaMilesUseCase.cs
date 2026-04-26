// src/modules/milescuenta/Application/UseCases/CrearCuentaMilesUseCase.cs
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milescuenta.Application.UseCases;

public sealed class CrearCuentaMilesUseCase
{
    private readonly IMilesCuentaRepository _repository;

    public CrearCuentaMilesUseCase(IMilesCuentaRepository repository)
        => _repository = repository;

    public async Task<MilesCuenta> ExecuteAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var yaExiste = await _repository.ExistsByClienteAsync(clienteId);
        if (yaExiste)
            throw new InvalidOperationException(
                $"El cliente {clienteId} ya tiene una cuenta de millas registrada.");

        var cuenta = MilesCuenta.Crear(clienteId);
        await _repository.SaveAsync(cuenta);
        return cuenta;
    }
}
