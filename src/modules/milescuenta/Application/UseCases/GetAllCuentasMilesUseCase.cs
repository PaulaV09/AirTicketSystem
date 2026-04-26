// src/modules/milescuenta/Application/UseCases/GetAllCuentasMilesUseCase.cs
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;

namespace AirTicketSystem.modules.milescuenta.Application.UseCases;

public sealed class GetAllCuentasMilesUseCase
{
    private readonly IMilesCuentaRepository _repository;

    public GetAllCuentasMilesUseCase(IMilesCuentaRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<MilesCuenta>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
