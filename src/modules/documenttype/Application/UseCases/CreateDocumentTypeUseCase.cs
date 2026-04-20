// src/modules/documenttype/Application/UseCases/CreateDocumentTypeUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public class CreateDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public CreateDocumentTypeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentTypeEntity> ExecuteAsync(string nombre)
    {
        var nombreVO = DescripcionDocumentType.Crear(nombre);

        if (await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de documento con el nombre '{nombreVO.Valor}'.");

        var entity = new DocumentTypeEntity { Descripcion = nombreVO.Valor };
        await _repository.AddAsync(entity);
        return entity;
    }
}