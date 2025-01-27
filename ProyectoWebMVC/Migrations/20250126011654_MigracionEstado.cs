using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class MigracionEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Solicitudes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Solicitudes");
        }
    }
}
