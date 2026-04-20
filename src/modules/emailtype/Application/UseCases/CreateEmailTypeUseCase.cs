// src/modules/emailtype/Application/UseCases/CreateEmailTypeUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public class CreateEmailTypeUseCase
{
    private readonly IEmailTypeRepository _repository;

    public CreateEmailTypeUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailTypeEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = DescripcionEmailType.Crear(nombre);

        if (await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de email con el nombre '{nombreVO.Valor}'.");

        var entity = new EmailTypeEntity { Descripcion = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}