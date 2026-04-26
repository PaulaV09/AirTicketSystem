// src/modules/milescuenta/Infrastructure/entity/MilesCuentaEntity.cs
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.milescuenta.Infrastructure.entity;

public class MilesCuentaEntity
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int SaldoActual { get; set; }
    public int MilesAcumuladasTotal { get; set; }
    public string Nivel { get; set; } = "BRONCE";
    public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

    public ClientEntity Cliente { get; set; } = null!;
}
