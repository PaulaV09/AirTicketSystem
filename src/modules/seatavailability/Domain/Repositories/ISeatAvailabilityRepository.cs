// src/modules/seatavailability/Domain/Repositories/ISeatAvailabilityRepository.cs
using AirTicketSystem.shared.contracts;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Domain.Repositories;

public interface ISeatAvailabilityRepository : IRepository<SeatAvailabilityEntity>
{
    Task<SeatAvailabilityEntity?> GetByVueloAndAsientoAsync(
        int vueloId, int asientoId);
    Task<IEnumerable<SeatAvailabilityEntity>> GetByVueloAsync(int vueloId);
    Task<IEnumerable<SeatAvailabilityEntity>> GetDisponiblesByVueloAsync(int vueloId);
    Task<IEnumerable<SeatAvailabilityEntity>> GetDisponiblesByVueloAndClaseAsync(
        int vueloId, int claseServicioId);
    Task<int> ContarDisponiblesByVueloAsync(int vueloId);
    Task<bool> AsientoDisponibleAsync(int vueloId, int asientoId);
    Task CrearDisponibilidadParaVueloAsync(int vueloId, int avionId);
}