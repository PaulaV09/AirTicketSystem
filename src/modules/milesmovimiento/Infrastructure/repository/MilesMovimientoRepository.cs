// src/modules/milesmovimiento/Infrastructure/repository/MilesMovimientoRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.milesmovimiento.Domain.aggregate;
using AirTicketSystem.modules.milesmovimiento.Domain.Repositories;
using AirTicketSystem.modules.milesmovimiento.Infrastructure.entity;

namespace AirTicketSystem.modules.milesmovimiento.Infrastructure.repository;

public sealed class MilesMovimientoRepository : IMilesMovimientoRepository
{
    private readonly AppDbContext _context;

    public MilesMovimientoRepository(AppDbContext context) => _context = context;

    public async Task<MilesMovimiento?> FindByIdAsync(int id)
    {
        var entity = await _context.MilesMovimientos.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<MilesMovimiento>> FindByCuentaAsync(int cuentaId)
    {
        var entities = await _context.MilesMovimientos
            .Where(m => m.CuentaId == cuentaId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<MilesMovimiento>> FindByReservaAsync(int reservaId)
    {
        var entities = await _context.MilesMovimientos
            .Where(m => m.ReservaId == reservaId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<MilesMovimiento>> FindAllAsync()
    {
        var entities = await _context.MilesMovimientos
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public Task<int> SumarAcumulacionesByCuentaAsync(int cuentaId)
        => _context.MilesMovimientos
            .Where(m => m.CuentaId == cuentaId && m.Tipo == "ACUMULACION")
            .SumAsync(m => (int?)m.Millas) // (int?) evita excepción si no hay filas
            .ContinueWith(t => t.Result ?? 0);

    public Task<int> SumarRedencionesByCuentaAsync(int cuentaId)
        => _context.MilesMovimientos
            .Where(m => m.CuentaId == cuentaId && m.Tipo == "REDENCION")
            .SumAsync(m => (int?)m.Millas)
            .ContinueWith(t => t.Result ?? 0);

    public async Task SaveAsync(MilesMovimiento movimiento)
    {
        var entity = MapToEntity(movimiento);
        _context.MilesMovimientos.Add(entity);
        await _context.SaveChangesAsync();
        movimiento.EstablecerId(entity.Id);
    }

    private static MilesMovimiento MapToDomain(MilesMovimientoEntity e) =>
        MilesMovimiento.Reconstituir(
            e.Id,
            e.CuentaId,
            e.ReservaId,
            e.Tipo,
            e.Millas,
            e.Fecha,
            e.Descripcion);

    private static MilesMovimientoEntity MapToEntity(MilesMovimiento m) => new()
    {
        CuentaId    = m.CuentaId,
        ReservaId   = m.ReservaId,
        Tipo        = m.Tipo.Valor,
        Millas      = m.Millas.Valor,
        Fecha       = m.Fecha.Valor,
        Descripcion = m.Descripcion.Valor
    };
}
