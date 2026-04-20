// src/modules/continent/Application/UseCases/UpdateContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;
using AirTicketSystem.modules.continent.Domain.ValueObjects;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public class UpdateContinentUseCase
{
    private readonly IContinentRepository _repository;

    public UpdateContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContinentEntity> ExecuteAsync(
        int id, string nombre, string codigo)
    {
        var continente = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");

        var nombreVO = NombreContinent.Crear(nombre);
        var codigoVO = CodigoContinent.Crear(codigo);

        // Verificar unicidad solo si el valor cambió
        if (codigoVO.Valor != continente.Codigo &&
            await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro continente con el código '{codigoVO.Valor}'.");

        if (nombreVO.Valor != continente.Nombre &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro continente con el nombre '{nombreVO.Valor}'.");

        continente.Nombre = nombreVO.Valor;
        continente.Codigo = codigoVO.Valor;

        await _repository.UpdateAsync(continente);
        return continente;
    }
}