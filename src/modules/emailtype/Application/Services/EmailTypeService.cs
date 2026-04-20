// src/modules/emailtype/Application/Services/EmailTypeService.cs
using AirTicketSystem.modules.emailtype.Application.Interfaces;
using AirTicketSystem.modules.emailtype.Application.UseCases;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Application.Services;

public class EmailTypeService : IEmailTypeService
{
    private readonly CreateEmailTypeUseCase _create;
    private readonly GetEmailTypeByIdUseCase _getById;
    private readonly GetAllEmailTypesUseCase _getAll;
    private readonly UpdateEmailTypeUseCase _update;
    private readonly DeleteEmailTypeUseCase _delete;

    public EmailTypeService(
        CreateEmailTypeUseCase create,
        GetEmailTypeByIdUseCase getById,
        GetAllEmailTypesUseCase getAll,
        UpdateEmailTypeUseCase update,
        DeleteEmailTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<EmailTypeEntity> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<EmailTypeEntity?> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id)!;

    public Task<IEnumerable<EmailTypeEntity>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<EmailTypeEntity> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}