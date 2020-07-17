using Microsoft.EntityFrameworkCore.Migrations;

namespace SanoCenterGold.Data.Migrations
{
    public partial class AutoIncrementAnadidoAIdUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
