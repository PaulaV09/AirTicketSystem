// src/modules/continent/Application/UseCases/CreateContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public class CreateContinentUseCase
{
    private readonly IContinentRepository _repository;

    public CreateContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContinentEntity> ExecuteAsync(string nombre, string codigo)
    {
        // Validamos con Value Objects — si algo está mal, lanzan excepción
        var nombreVO = NombreContinent.Crear(nombre);
        var codigoVO = CodigoContinent.Crear(codigo);

        // Verificamos unicidad antes de persistir
        if (await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un continente con el código '{codigoVO.Valor}'.");

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un continente con el nombre '{nombreVO.Valor}'.");

        var entity = new ContinentEntity
        {
            Nombre = nombreVO.Valor,
            Codigo = codigoVO.Valor
        };

        await _repository.AddAsync(entity);
        return entity;
    }
}