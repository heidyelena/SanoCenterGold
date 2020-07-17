using Microsoft.EntityFrameworkCore.Migrations;

namespace SanoCenterGold.Data.Migrations
{
    public partial class AñadirEstadoREto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoDelReto",
                table: "RetoUsuario",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoDelReto",
                table: "RetoUsuario");
        }
    }
}
