using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirTicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMilesEnPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "miles_usadas",
                table: "pagos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "miles_usadas",
                table: "pagos");
        }
    }
}
