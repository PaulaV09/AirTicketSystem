// src/modules/documenttype/Application/UseCases/UpdateDocumentTypeUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public class UpdateDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public UpdateDocumentTypeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentTypeEntity> ExecuteAsync(int id, string nombre)
    {
        var tipoDocumento = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {id}.");

        var nombreVO = DescripcionDocumentType.Crear(nombre);

        if (nombreVO.Valor != tipoDocumento.Descripcion &&
            await _repository.ExistsByDescripcionAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de documento con la descripción '{nombreVO.Valor}'.");

        tipoDocumento.Descripcion = nombreVO.Valor;
        await _repository.UpdateAsync(tipoDocumento);
        return tipoDocumento;
    }
}