using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCuentasMillas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cuentas_millas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    saldo_actual = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    miles_acumuladas_total = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    nivel = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "BRONCE")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inscripcion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuentas_millas", x => x.id);
                    table.ForeignKey(
                        name: "FK_cuentas_millas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "uq_cuentas_millas_cliente",
                table: "cuentas_millas",
                column: "cliente_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cuentas_millas");
        }
    }
}
