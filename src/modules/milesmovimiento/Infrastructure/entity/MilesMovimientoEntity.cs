// src/modules/milesmovimiento/Infrastructure/entity/MilesMovimientoEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.milescuenta.Infrastructure.entity;

namespace AirTicketSystem.modules.milesmovimiento.Infrastructure.entity;

public class MilesMovimientoEntity
{
    public int Id { get; set; }
    public int CuentaId { get; set; }
    public int? ReservaId { get; set; }
    public string Tipo { get; set; } = null!;
    public int Millas { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Descripcion { get; set; } = null!;

    public MilesCuentaEntity Cuenta { get; set; } = null!;
    public BookingEntity? Reserva { get; set; }
}
