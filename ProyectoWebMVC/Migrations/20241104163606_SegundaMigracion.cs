using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    LugarResidencia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ingresos = table.Column<int>(type: "int", nullable: false),
                    AmbienteFamiliar = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fecha = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.IdSolicitud);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitudes");
        }
    }
}
