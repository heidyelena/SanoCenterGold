using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanoCenterGold.Data.Migrations
{
    public partial class AñadirNullAFechaCompletado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCompletado",
                table: "RetoUsuario",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCompletado",
                table: "RetoUsuario",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
