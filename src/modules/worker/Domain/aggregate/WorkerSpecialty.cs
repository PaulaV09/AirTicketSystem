// src/modules/worker/Domain/aggregate/WorkerSpecialty.cs
namespace AirTicketSystem.modules.worker.Domain.aggregate;

public sealed class WorkerSpecialty
{
    public int Id { get; private set; }
    public int TrabajadorId { get; private set; }
    public int EspecialidadId { get; private set; }

    private WorkerSpecialty() { }

    public static WorkerSpecialty Crear(int trabajadorId, int especialidadId)
    {
        if (trabajadorId <= 0)
            throw new ArgumentException("El trabajador es obligatorio.");

        if (especialidadId <= 0)
            throw new ArgumentException("La especialidad es obligatoria.");

        if (trabajadorId == especialidadId)
            throw new InvalidOperationException(
                "El ID de trabajador y especialidad no pueden ser iguales.");

        return new WorkerSpecialty
        {
            TrabajadorId  = trabajadorId,
            EspecialidadId = especialidadId
        };
    }

    public override string ToString() =>
        $"Trabajador #{TrabajadorId} — Especialidad #{EspecialidadId}";
}