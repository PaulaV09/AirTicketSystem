// src/modules/documenttype/Domain/aggregate/DocumentType.cs
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Domain.aggregate;

public sealed class DocumentType
{
    public int Id { get; private set; }
    public DescripcionDocumentType Descripcion { get; private set; } = null!;

    private DocumentType() { }

    public static DocumentType Crear(string descripcion)
    {
        return new DocumentType
        {
            Descripcion = DescripcionDocumentType.Crear(descripcion)
        };
    }

    public void ActualizarDescripcion(string descripcion)
    {
        Descripcion = DescripcionDocumentType.Crear(descripcion);
    }

    public override string ToString() => Descripcion.ToString();
}