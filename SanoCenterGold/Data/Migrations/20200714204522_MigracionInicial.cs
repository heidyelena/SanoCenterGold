using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanoCenterGold.Data.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GimnasioId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdGimnasio",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetosCompletados",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Telefono",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ejercicio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    Dificultad = table.Column<int>(nullable: false),
                    Repeticiones = table.Column<int>(nullable: false),
                    Series = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejercicio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gimnasio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    Localidad = table.Column<string>(nullable: true),
                    Telefono = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gimnasio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    Imagen = table.Column<string>(nullable: true),
                    FechaLimite = table.Column<DateTime>(nullable: false),
                    IdEntrenador = table.Column<int>(nullable: false),
                    EntrenadorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reto_AspNetUsers_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RetoEjercicio",
                columns: table => new
                {
                    IdRetoEjercicio = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReto = table.Column<int>(nullable: false),
                    RetoId = table.Column<int>(nullable: true),
                    IdEjercicio = table.Column<int>(nullable: false),
                    EjercicioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetoEjercicio", x => x.IdRetoEjercicio);
                    table.ForeignKey(
                        name: "FK_RetoEjercicio_Ejercicio_EjercicioId",
                        column: x => x.EjercicioId,
                        principalTable: "Ejercicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RetoEjercicio_Reto_RetoId",
                        column: x => x.RetoId,
                        principalTable: "Reto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RetoUsuario",
                columns: table => new
                {
                    IdRetoUsuario = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReto = table.Column<int>(nullable: false),
                    RetoId = table.Column<int>(nullable: true),
                    IdUsuario = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    FechaCompletado = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetoUsuario", x => x.IdRetoUsuario);
                    table.ForeignKey(
                        name: "FK_RetoUsuario_Reto_RetoId",
                        column: x => x.RetoId,
                        principalTable: "Reto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RetoUsuario_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Valoracion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Puntuacion = table.Column<int>(nullable: false),
                    Comentario = table.Column<string>(nullable: true),
                    IdReto = table.Column<int>(nullable: false),
                    RetoId = table.Column<int>(nullable: true),
                    IdUsuario = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valoracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valoracion_Reto_RetoId",
                        column: x => x.RetoId,
                        principalTable: "Reto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Valoracion_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GimnasioId",
                table: "AspNetUsers",
                column: "GimnasioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reto_EntrenadorId",
                table: "Reto",
                column: "EntrenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_RetoEjercicio_EjercicioId",
                table: "RetoEjercicio",
                column: "EjercicioId");

            migrationBuilder.CreateIndex(
                name: "IX_RetoEjercicio_RetoId",
                table: "RetoEjercicio",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_RetoUsuario_RetoId",
                table: "RetoUsuario",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_RetoUsuario_UsuarioId",
                table: "RetoUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Valoracion_RetoId",
                table: "Valoracion",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Valoracion_UsuarioId",
                table: "Valoracion",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Gimnasio_GimnasioId",
                table: "AspNetUsers",
                column: "GimnasioId",
                principalTable: "Gimnasio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Gimnasio_GimnasioId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Gimnasio");

            migrationBuilder.DropTable(
                name: "RetoEjercicio");

            migrationBuilder.DropTable(
                name: "RetoUsuario");

            migrationBuilder.DropTable(
                name: "Valoracion");

            migrationBuilder.DropTable(
                name: "Ejercicio");

            migrationBuilder.DropTable(
                name: "Reto");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GimnasioId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dni",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GimnasioId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdGimnasio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Localidad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RetosCompletados",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "AspNetUsers");
        }
    }
}
