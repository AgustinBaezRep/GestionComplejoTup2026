using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComplejo.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddVestuarioAndServicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostoAdicional = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vestuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroVestuarios = table.Column<int>(type: "int", nullable: false),
                    TieneDuchas = table.Column<bool>(type: "bit", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    CanchaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vestuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vestuarios_Canchas_CanchaId",
                        column: x => x.CanchaId,
                        principalTable: "Canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CanchaServicio",
                columns: table => new
                {
                    CanchasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiciosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanchaServicio", x => new { x.CanchasId, x.ServiciosId });
                    table.ForeignKey(
                        name: "FK_CanchaServicio_Canchas_CanchasId",
                        column: x => x.CanchasId,
                        principalTable: "Canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanchaServicio_Servicios_ServiciosId",
                        column: x => x.ServiciosId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanchaServicio_ServiciosId",
                table: "CanchaServicio",
                column: "ServiciosId");

            migrationBuilder.CreateIndex(
                name: "IX_Vestuarios_CanchaId",
                table: "Vestuarios",
                column: "CanchaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanchaServicio");

            migrationBuilder.DropTable(
                name: "Vestuarios");

            migrationBuilder.DropTable(
                name: "Servicios");
        }
    }
}
