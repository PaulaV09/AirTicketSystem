// src/modules/emailtype/Application/UseCases/GetEmailTypeByIdUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public class GetEmailTypeByIdUseCase
{
    private readonly IEmailTypeRepository _repository;

    public GetEmailTypeByIdUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailTypeEntity> ExecuteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de email no es válido.");

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {id}.");
    }
}