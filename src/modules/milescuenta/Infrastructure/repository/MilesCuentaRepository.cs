// src/modules/milescuenta/Infrastructure/repository/MilesCuentaRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.milescuenta.Domain.aggregate;
using AirTicketSystem.modules.milescuenta.Domain.Repositories;
using AirTicketSystem.modules.milescuenta.Infrastructure.entity;

namespace AirTicketSystem.modules.milescuenta.Infrastructure.repository;

public sealed class MilesCuentaRepository : IMilesCuentaRepository
{
    private readonly AppDbContext _context;

    public MilesCuentaRepository(AppDbContext context) => _context = context;

    public async Task<MilesCuenta?> FindByIdAsync(int id)
    {
        var entity = await _context.CuentasMillas.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<MilesCuenta?> FindByClienteAsync(int clienteId)
    {
        var entity = await _context.CuentasMillas
            .FirstOrDefaultAsync(m => m.ClienteId == clienteId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<MilesCuenta>> FindAllAsync()
    {
        var entities = await _context.CuentasMillas
            .OrderByDescending(m => m.SaldoActual)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsByClienteAsync(int clienteId)
        => _context.CuentasMillas.AnyAsync(m => m.ClienteId == clienteId);

    public async Task SaveAsync(MilesCuenta cuenta)
    {
        var entity = MapToEntity(cuenta);
        _context.CuentasMillas.Add(entity);
        await _context.SaveChangesAsync();
        cuenta.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(MilesCuenta cuenta)
    {
        var entity = await _context.CuentasMillas.FindAsync(cuenta.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la cuenta de millas con ID {cuenta.Id}.");

        entity.SaldoActual          = cuenta.SaldoActual.Valor;
        entity.MilesAcumuladasTotal = cuenta.MilesAcumuladasTotal;
        entity.Nivel                = cuenta.Nivel.Valor;

        await _context.SaveChangesAsync();
    }

    private static MilesCuenta MapToDomain(MilesCuentaEntity e) =>
        MilesCuenta.Reconstituir(
            e.Id,
            e.ClienteId,
            e.SaldoActual,
            e.MilesAcumuladasTotal,
            e.Nivel,
            e.FechaInscripcion);

    private static MilesCuentaEntity MapToEntity(MilesCuenta m) => new()
    {
        ClienteId            = m.ClienteId,
        SaldoActual          = m.SaldoActual.Valor,
        MilesAcumuladasTotal = m.MilesAcumuladasTotal,
        Nivel                = m.Nivel.Valor,
        FechaInscripcion     = m.FechaInscripcion.Valor
    };
}
