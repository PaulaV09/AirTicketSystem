// src/modules/milescuenta/Infrastructure/entity/MilesCuentaEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.milescuenta.Infrastructure.entity;

public class MilesCuentaEntityConfig : IEntityTypeConfiguration<MilesCuentaEntity>
{
    public void Configure(EntityTypeBuilder<MilesCuentaEntity> builder)
    {
        builder.ToTable("cuentas_millas");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.ClienteId)
            .HasColumnName("cliente_id")
            .IsRequired();

        builder.Property(m => m.SaldoActual)
            .HasColumnName("saldo_actual")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(m => m.MilesAcumuladasTotal)
            .HasColumnName("miles_acumuladas_total")
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(m => m.Nivel)
            .HasColumnName("nivel")
            .HasMaxLength(10)
            .IsRequired()
            .HasDefaultValue("BRONCE");

        builder.Property(m => m.FechaInscripcion)
            .HasColumnName("fecha_inscripcion")
            .IsRequired();

        // Un cliente solo puede tener UNA cuenta de millas
        builder.HasIndex(m => m.ClienteId)
            .IsUnique()
            .HasDatabaseName("uq_cuentas_millas_cliente");

        builder.HasOne(m => m.Cliente)
            .WithMany()
            .HasForeignKey(m => m.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
