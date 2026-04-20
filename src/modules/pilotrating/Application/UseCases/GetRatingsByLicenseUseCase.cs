// src/modules/pilotrating/Application/UseCases/GetRatingsByLicenseUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public class GetRatingsByLicenseUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetRatingsByLicenseUseCase(IPilotRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PilotRatingEntity>> ExecuteAsync(int licenciaId)
    {
        if (licenciaId <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        return await _repository.GetByLicenciaAsync(licenciaId);
    }
}