// src/modules/worker/Infrastructure/repository/WorkerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.repository;

public class WorkerRepository : BaseRepository<WorkerEntity>, IWorkerRepository
{
    public WorkerRepository(AppDbContext context) : base(context) { }

    public async Task<WorkerEntity?> GetByPersonaAsync(int personaId)
        => await _dbSet
            .Include(w => w.TipoTrabajador)
            .Include(w => w.Persona)
            .FirstOrDefaultAsync(w => w.PersonaId == personaId);

    public async Task<WorkerEntity?> GetByUsuarioAsync(int usuarioId)
        => await _dbSet
            .Include(w => w.Persona)
            .Include(w => w.TipoTrabajador)
            .FirstOrDefaultAsync(w => w.UsuarioId == usuarioId);

    public async Task<IEnumerable<WorkerEntity>> GetByAerolineaAsync(int aerolineaId)
        => await _dbSet
            .Include(w => w.Persona)
            .Include(w => w.TipoTrabajador)
            .Where(w => w.AerolineaId == aerolineaId && w.Activo)
            .OrderBy(w => w.Persona.Apellidos)
            .ToListAsync();

    public async Task<IEnumerable<WorkerEntity>> GetByAeropuertoBaseAsync(int aeropuertoId)
        => await _dbSet
            .Include(w => w.Persona)
            .Include(w => w.TipoTrabajador)
            .Where(w => w.AeropuertoBaseId == aeropuertoId && w.Activo)
            .OrderBy(w => w.Persona.Apellidos)
            .ToListAsync();

    public async Task<IEnumerable<WorkerEntity>> GetByTipoTrabajadorAsync(
        int tipoTrabajadorId)
        => await _dbSet
            .Include(w => w.Persona)
            .Where(w => w.TipoTrabajadorId == tipoTrabajadorId && w.Activo)
            .OrderBy(w => w.Persona.Apellidos)
            .ToListAsync();

    public async Task<IEnumerable<WorkerEntity>> GetActivosAsync()
        => await _dbSet
            .Include(w => w.Persona)
            .Include(w => w.TipoTrabajador)
            .Where(w => w.Activo)
            .OrderBy(w => w.Persona.Apellidos)
            .ToListAsync();

    public async Task<IEnumerable<WorkerEntity>> GetPilotosHabilitadosParaModeloAsync(
        int modeloAvionId)
        => await _dbSet
            .Include(w => w.Persona)
            .Include(w => w.Licencias)
                .ThenInclude(l => l.Habilitaciones)
            .Where(w =>
                w.Activo &&
                w.Licencias.Any(l =>
                    l.Activa &&
                    l.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today) &&
                    l.Habilitaciones.Any(h =>
                        h.ModeloAvionId == modeloAvionId &&
                        h.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today))))
            .ToListAsync();
}