using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foro.Migrations
{
    public partial class Foro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    Privada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entradas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entradas_Usuarios_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MiembrosHabilitados",
                columns: table => new
                {
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    EntradaId = table.Column<int>(type: "int", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiembrosHabilitados", x => new { x.MiembroId, x.EntradaId });
                    table.ForeignKey(
                        name: "FK_MiembrosHabilitados_Entradas_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entradas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MiembrosHabilitados_Usuarios_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    PreguntaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    EntradaId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.PreguntaId);
                    table.ForeignKey(
                        name: "FK_Preguntas_Entradas_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entradas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Preguntas_Usuarios_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    RespuestaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreguntaId = table.Column<int>(type: "int", nullable: false),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.RespuestaId);
                    table.ForeignKey(
                        name: "FK_Respuestas_Preguntas_PreguntaId",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "PreguntaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuestas_Usuarios_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reacciones",
                columns: table => new
                {
                    RespuestaId = table.Column<int>(type: "int", nullable: false),
                    MiembroId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MeGusta = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reacciones", x => new { x.MiembroId, x.RespuestaId });
                    table.ForeignKey(
                        name: "FK_Reacciones_Respuestas_RespuestaId",
                        column: x => x.RespuestaId,
                        principalTable: "Respuestas",
                        principalColumn: "RespuestaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reacciones_Usuarios_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_CategoriaId",
                table: "Entradas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entradas_MiembroId",
                table: "Entradas",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_MiembrosHabilitados_EntradaId",
                table: "MiembrosHabilitados",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_EntradaId",
                table: "Preguntas",
                column: "EntradaId");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_MiembroId",
                table: "Preguntas",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Reacciones_RespuestaId",
                table: "Reacciones",
                column: "RespuestaId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_MiembroId",
                table: "Respuestas",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_PreguntaId",
                table: "Respuestas",
                column: "PreguntaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MiembrosHabilitados");

            migrationBuilder.DropTable(
                name: "Reacciones");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Entradas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
