using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMigracionfoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPerfilPath",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfilPath",
                table: "Usuarios");
        }
    }
}
