// src/modules/flight/Domain/aggregate/Flight.cs
using AirTicketSystem.modules.flight.Domain.ValueObjects;

namespace AirTicketSystem.modules.flight.Domain.aggregate;

public sealed class Flight
{
    public int Id { get; private set; }
    public int RutaId { get; private set; }
    public int AvionId { get; private set; }
    public int? PuertaEmbarqueId { get; private set; }
    public NumeroVueloFlight NumeroVuelo { get; private set; } = null!;
    public FechaSalidaFlight FechaSalida { get; private set; } = null!;
    public FechaLlegadaEstimadaFlight FechaLlegadaEstimada { get; private set; } = null!;
    public FechaLlegadaRealFlight? FechaLlegadaReal { get; private set; }
    public EstadoFlight Estado { get; private set; } = null!;
    public MotivoCambioEstadoFlight? MotivoCambioEstado { get; private set; }
    public CheckinAperturaFlight? CheckinApertura { get; private set; }
    public CheckinCierreFlight? CheckinCierre { get; private set; }

    private Flight() { }

    public static Flight Crear(
        int rutaId,
        int avionId,
        string numeroVuelo,
        DateTime fechaSalida,
        DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId = null)
    {
        if (rutaId <= 0)
            throw new ArgumentException("La ruta es obligatoria.");

        if (avionId <= 0)
            throw new ArgumentException("El avión es obligatorio.");

        var salida   = FechaSalidaFlight.Crear(fechaSalida);
        var llegada  = FechaLlegadaEstimadaFlight.Crear(
            fechaLlegadaEstimada, fechaSalida);

        return new Flight
        {
            RutaId               = rutaId,
            AvionId              = avionId,
            PuertaEmbarqueId     = puertaEmbarqueId,
            NumeroVuelo          = NumeroVueloFlight.Crear(numeroVuelo),
            FechaSalida          = salida,
            FechaLlegadaEstimada = llegada,
            FechaLlegadaReal     = null,
            Estado               = EstadoFlight.Programado(),
            MotivoCambioEstado   = null,
            CheckinApertura      = null,
            CheckinCierre        = null
        };
    }

    // ── Gestión de check-in ──────────────────────────────────

    public void AbrirCheckin(DateTime apertura, DateTime cierre)
    {
        if (!Estado.PermiteCheckin)
            throw new InvalidOperationException(
                $"No se puede abrir el check-in con el vuelo en estado '{Estado}'.");

        CheckinApertura = CheckinAperturaFlight.Crear(apertura, FechaSalida.Valor);
        CheckinCierre   = CheckinCierreFlight.Crear(
            cierre, FechaSalida.Valor, apertura);
    }

    public void CerrarCheckin()
    {
        if (CheckinApertura is null)
            throw new InvalidOperationException(
                "No se puede cerrar un check-in que no ha sido abierto.");

        if (CheckinCierre is not null && CheckinCierre.EstaCerrado)
            throw new InvalidOperationException(
                "El check-in ya se encuentra cerrado.");
    }

    // ── Gestión de puerta ────────────────────────────────────

    public void AsignarPuerta(int puertaEmbarqueId)
    {
        if (puertaEmbarqueId <= 0)
            throw new ArgumentException("La puerta de embarque no es válida.");

        if (Estado.EstaFinalizado)
            throw new InvalidOperationException(
                "No se puede asignar puerta a un vuelo finalizado.");

        PuertaEmbarqueId = puertaEmbarqueId;
    }

    public void LiberarPuerta()
    {
        if (PuertaEmbarqueId is null)
            throw new InvalidOperationException(
                "El vuelo no tiene una puerta asignada.");

        PuertaEmbarqueId = null;
    }

    // ── Máquina de estados ───────────────────────────────────

    public void IniciarAbordaje(string? motivo = null)
    {
        CambiarEstado(EstadoFlight.Abordando(), motivo);
    }

    public void IniciarVuelo(string? motivo = null)
    {
        CambiarEstado(EstadoFlight.EnVuelo(), motivo);
    }

    public void RegistrarAterrizaje(DateTime fechaLlegadaReal, string? motivo = null)
    {
        CambiarEstado(EstadoFlight.Aterrizado(), motivo);
        FechaLlegadaReal = FechaLlegadaRealFlight.Crear(
            fechaLlegadaReal, FechaSalida.Valor);
    }

    public void Cancelar(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "Se debe indicar el motivo de cancelación del vuelo.");

        CambiarEstado(EstadoFlight.Cancelado(), motivo);
    }

    public void Demorar(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "Se debe indicar el motivo de la demora.");

        CambiarEstado(EstadoFlight.Demorado(), motivo);
    }

    public void Desviar(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException(
                "Se debe indicar el motivo del desvío.");

        CambiarEstado(EstadoFlight.Desviado(), motivo);
    }

    private void CambiarEstado(EstadoFlight nuevoEstado, string? motivo)
    {
        if (!Estado.PuedeTransicionarA(nuevoEstado))
            throw new InvalidOperationException(
                $"No se puede cambiar el estado de '{Estado}' a '{nuevoEstado}'.");

        Estado             = nuevoEstado;
        MotivoCambioEstado = motivo is not null
            ? MotivoCambioEstadoFlight.Crear(motivo)
            : null;
    }

    // ── Propiedades de negocio ───────────────────────────────

    public bool CheckinEstaAbierto =>
        CheckinApertura is not null &&
        CheckinCierre is not null &&
        CheckinApertura.EstaAbierto &&
        !CheckinCierre.EstaCerrado;

    public bool AceptaNuevasReservas =>
        Estado.PermiteNuevasReservas;

    public bool EstaFinalizado => Estado.EstaFinalizado;

    public bool LlegoATiempo =>
        FechaLlegadaReal is not null &&
        FechaLlegadaReal.LlegoATiempo(FechaLlegadaEstimada.Valor);

    public int? MinutosDeDemora =>
        FechaLlegadaReal?.MinutosDeDemora(FechaLlegadaEstimada.Valor);

    public override string ToString() =>
        $"[{NumeroVuelo}] {FechaSalida.EnFormatoCorto} — {Estado}";
}