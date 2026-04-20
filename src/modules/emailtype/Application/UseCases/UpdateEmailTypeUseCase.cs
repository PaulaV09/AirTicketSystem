// src/modules/emailtype/Application/UseCases/UpdateEmailTypeUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public class UpdateEmailTypeUseCase
{
    private readonly IEmailTypeRepository _repository;

    public UpdateEmailTypeUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailTypeEntity> ExecuteAsync(int id, string nombre)
    {
        var tipoEmail = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {id}.");

        var nombreVO = DescripcionEmailType.Crear(nombre);

        if (nombreVO.Valor != tipoEmail.Descripcion &&
            await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de email con la descripción '{nombreVO.Valor}'.");

        tipoEmail.Descripcion = nombreVO.Valor;
        await _repository.UpdateAsync(tipoEmail);
        return tipoEmail;
    }
}