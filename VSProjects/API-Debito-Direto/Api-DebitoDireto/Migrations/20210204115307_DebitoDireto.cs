using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api_DebitoDireto.Migrations
{
    public partial class DebitoDireto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DebitoDireto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iban = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    codigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    freguesia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nifDestinatario = table.Column<int>(type: "int", nullable: false),
                    custo = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebitoDireto", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebitoDireto");
        }
    }
}
