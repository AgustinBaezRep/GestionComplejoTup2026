using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComplejo.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoPisoToCancha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Piso",
                table: "Canchas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Piso",
                table: "Canchas");
        }
    }
}
