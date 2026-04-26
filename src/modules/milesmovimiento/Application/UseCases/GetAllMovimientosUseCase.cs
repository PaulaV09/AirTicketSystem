// src/modules/milesmovimiento/Application/UseCases/GetAllMovimientosUseCase.cs
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;

namespace AirTicketSystem.modules.milesmovimiento.Application.UseCases;

public sealed class GetAllMovimientosUseCase
{
    private readonly IMilesMovimientoRepository _repository;

    public GetAllMovimientosUseCase(IMilesMovimientoRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<MilesMovimiento>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
