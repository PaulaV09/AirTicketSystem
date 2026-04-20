// src/modules/serviceclass/Application/UseCases/CreateServiceClassUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;
using AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class CreateServiceClassUseCase
{
    private readonly IServiceClassRepository _repository;

    public CreateServiceClassUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceClassEntity> ExecuteAsync(
        string nombre, string codigo, string? descripcion)
    {
        var nombreVO = NombreServiceClass.Crear(nombre);
        var codigoVO = CodigoServiceClass.Crear(codigo);

        if (await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una clase de servicio con el código '{codigoVO.Valor}'.");

        var entity = new ServiceClassEntity
        {
            Nombre      = nombreVO.Valor,
            Codigo      = codigoVO.Valor,
            Descripcion = descripcion is not null
                ? DescripcionServiceClass.Crear(descripcion).Valor
                : null
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}