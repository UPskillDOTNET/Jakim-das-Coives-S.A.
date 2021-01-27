using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Parque_Publico.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Nif = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Nif);
                });

            migrationBuilder.CreateTable(
                name: "Freguesias",
                columns: table => new
                {
                    FreguesiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Freguesias", x => x.FreguesiaId);
                });

            migrationBuilder.CreateTable(
                name: "Parques",
                columns: table => new
                {
                    ParqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreguesiaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parques", x => x.ParqueId);
                    table.ForeignKey(
                        name: "FK_Parques_Freguesias_FreguesiaId",
                        column: x => x.FreguesiaId,
                        principalTable: "Freguesias",
                        principalColumn: "FreguesiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    LugarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParqueId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Fila = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Andar = table.Column<int>(type: "int", nullable: false),
                    Preco = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.LugarId);
                    table.ForeignKey(
                        name: "FK_Lugares_Parques_ParqueId",
                        column: x => x.ParqueId,
                        principalTable: "Parques",
                        principalColumn: "ParqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    ReservaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NifCliente = table.Column<int>(type: "int", nullable: false),
                    LugarId = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_NifCliente",
                        column: x => x.NifCliente,
                        principalTable: "Clientes",
                        principalColumn: "Nif",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Lugares_LugarId",
                        column: x => x.LugarId,
                        principalTable: "Lugares",
                        principalColumn: "LugarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_ParqueId",
                table: "Lugares",
                column: "ParqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Parques_FreguesiaId",
                table: "Parques",
                column: "FreguesiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_LugarId",
                table: "Reservas",
                column: "LugarId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_NifCliente",
                table: "Reservas",
                column: "NifCliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropTable(
                name: "Parques");

            migrationBuilder.DropTable(
                name: "Freguesias");
        }
    }
}
