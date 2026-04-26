// src/modules/milesmovimiento/Application/DTOs/ResumenAcumulacionMiles.cs
namespace AirTicketSystem.modules.milesmovimiento.Application.DTOs;

// Resultado del proceso de acumulación masiva por vuelo.
// No es un aggregate ni un VO — es un DTO de salida de use case.
public sealed record ResumenAcumulacionMiles(
    int VueloId,
    string NumeroVuelo,
    int ReservasConfirmadas,
    int ClientesProcesados,
    int TotalMilesAcumuladas,
    IReadOnlyList<string> Advertencias)
{
    public bool TuvoAdvertencias => Advertencias.Count > 0;

    public override string ToString() =>
        $"Vuelo {NumeroVuelo}: {ClientesProcesados}/{ReservasConfirmadas} clientes " +
        $"procesados | {TotalMilesAcumuladas:N0} millas distribuidas";
}
