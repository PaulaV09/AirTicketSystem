// src/modules/specialty/Application/Interfaces/ISpecialtyService.cs
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Application.Interfaces;

public interface ISpecialtyService
{
    Task<SpecialtyEntity> CreateAsync(string nombre, int? tipoTrabajadorId);
    Task<SpecialtyEntity?> GetByIdAsync(int id);
    Task<IEnumerable<SpecialtyEntity>> GetAllAsync();
    Task<IEnumerable<SpecialtyEntity>> GetByWorkerTypeAsync(int tipoTrabajadorId);
    Task<IEnumerable<SpecialtyEntity>> GetGeneralesAsync();
    Task<SpecialtyEntity> UpdateAsync(
        int id, string nombre, int? tipoTrabajadorId);
    Task DeleteAsync(int id);
}