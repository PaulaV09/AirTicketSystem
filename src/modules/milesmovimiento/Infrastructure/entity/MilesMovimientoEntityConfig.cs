// src/modules/milesmovimiento/Infrastructure/entity/MilesMovimientoEntityConfig.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirTicketSystem.modules.milesmovimiento.Infrastructure.entity;

public class MilesMovimientoEntityConfig : IEntityTypeConfiguration<MilesMovimientoEntity>
{
    public void Configure(EntityTypeBuilder<MilesMovimientoEntity> builder)
    {
        builder.ToTable("miles_movimientos");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.CuentaId)
            .HasColumnName("cuenta_id")
            .IsRequired();

        builder.Property(m => m.ReservaId)
            .HasColumnName("reserva_id")
            .IsRequired(false);

        builder.Property(m => m.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(m => m.Millas)
            .HasColumnName("millas")
            .IsRequired();

        builder.Property(m => m.Fecha)
            .HasColumnName("fecha")
            .IsRequired();

        builder.Property(m => m.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(m => m.Cuenta)
            .WithMany()
            .HasForeignKey(m => m.CuentaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Reserva)
            .WithMany()
            .HasForeignKey(m => m.ReservaId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
