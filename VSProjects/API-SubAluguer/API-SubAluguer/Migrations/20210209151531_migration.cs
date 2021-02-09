using Microsoft.EntityFrameworkCore.Migrations;

namespace API_SubAluguer.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disponibilidade_Reserva_ReservaId",
                table: "Disponibilidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugar_Parque_ParqueId",
                table: "Lugar");

            migrationBuilder.DropForeignKey(
                name: "FK_Parque_Freguesia_FreguesiaId",
                table: "Parque");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Cliente_NifCliente",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Lugar_LugarId",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parque",
                table: "Parque");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lugar",
                table: "Lugar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Freguesia",
                table: "Freguesia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disponibilidade",
                table: "Disponibilidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente");

            migrationBuilder.RenameTable(
                name: "Reserva",
                newName: "Reservas");

            migrationBuilder.RenameTable(
                name: "Parque",
                newName: "Parques");

            migrationBuilder.RenameTable(
                name: "Lugar",
                newName: "Lugares");

            migrationBuilder.RenameTable(
                name: "Freguesia",
                newName: "Freguesias");

            migrationBuilder.RenameTable(
                name: "Disponibilidade",
                newName: "Disponibilidades");

            migrationBuilder.RenameTable(
                name: "Cliente",
                newName: "Clientes");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_NifCliente",
                table: "Reservas",
                newName: "IX_Reservas_NifCliente");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_LugarId",
                table: "Reservas",
                newName: "IX_Reservas_LugarId");

            migrationBuilder.RenameIndex(
                name: "IX_Parque_FreguesiaId",
                table: "Parques",
                newName: "IX_Parques_FreguesiaId");

            migrationBuilder.RenameIndex(
                name: "IX_Lugar_ParqueId",
                table: "Lugares",
                newName: "IX_Lugares_ParqueId");

            migrationBuilder.RenameIndex(
                name: "IX_Disponibilidade_ReservaId",
                table: "Disponibilidades",
                newName: "IX_Disponibilidades_ReservaId");

            migrationBuilder.AlterColumn<string>(
                name: "Rua",
                table: "Parques",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parques",
                table: "Parques",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lugares",
                table: "Lugares",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Freguesias",
                table: "Freguesias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disponibilidades",
                table: "Disponibilidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Disponibilidades_Reservas_ReservaId",
                table: "Disponibilidades",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_Parques_ParqueId",
                table: "Lugares",
                column: "ParqueId",
                principalTable: "Parques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parques_Freguesias_FreguesiaId",
                table: "Parques",
                column: "FreguesiaId",
                principalTable: "Freguesias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_NifCliente",
                table: "Reservas",
                column: "NifCliente",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Lugares_LugarId",
                table: "Reservas",
                column: "LugarId",
                principalTable: "Lugares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Disponibilidades_Reservas_ReservaId",
                table: "Disponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_Parques_ParqueId",
                table: "Lugares");

            migrationBuilder.DropForeignKey(
                name: "FK_Parques_Freguesias_FreguesiaId",
                table: "Parques");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_NifCliente",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Lugares_LugarId",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parques",
                table: "Parques");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lugares",
                table: "Lugares");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Freguesias",
                table: "Freguesias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disponibilidades",
                table: "Disponibilidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Reservas",
                newName: "Reserva");

            migrationBuilder.RenameTable(
                name: "Parques",
                newName: "Parque");

            migrationBuilder.RenameTable(
                name: "Lugares",
                newName: "Lugar");

            migrationBuilder.RenameTable(
                name: "Freguesias",
                newName: "Freguesia");

            migrationBuilder.RenameTable(
                name: "Disponibilidades",
                newName: "Disponibilidade");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "Cliente");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_NifCliente",
                table: "Reserva",
                newName: "IX_Reserva_NifCliente");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_LugarId",
                table: "Reserva",
                newName: "IX_Reserva_LugarId");

            migrationBuilder.RenameIndex(
                name: "IX_Parques_FreguesiaId",
                table: "Parque",
                newName: "IX_Parque_FreguesiaId");

            migrationBuilder.RenameIndex(
                name: "IX_Lugares_ParqueId",
                table: "Lugar",
                newName: "IX_Lugar_ParqueId");

            migrationBuilder.RenameIndex(
                name: "IX_Disponibilidades_ReservaId",
                table: "Disponibilidade",
                newName: "IX_Disponibilidade_ReservaId");

            migrationBuilder.AlterColumn<string>(
                name: "Rua",
                table: "Parque",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Cliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parque",
                table: "Parque",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lugar",
                table: "Lugar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Freguesia",
                table: "Freguesia",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disponibilidade",
                table: "Disponibilidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cliente",
                table: "Cliente",
                column: "Nif");

            migrationBuilder.AddForeignKey(
                name: "FK_Disponibilidade_Reserva_ReservaId",
                table: "Disponibilidade",
                column: "ReservaId",
                principalTable: "Reserva",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lugar_Parque_ParqueId",
                table: "Lugar",
                column: "ParqueId",
                principalTable: "Parque",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parque_Freguesia_FreguesiaId",
                table: "Parque",
                column: "FreguesiaId",
                principalTable: "Freguesia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Cliente_NifCliente",
                table: "Reserva",
                column: "NifCliente",
                principalTable: "Cliente",
                principalColumn: "Nif",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Lugar_LugarId",
                table: "Reserva",
                column: "LugarId",
                principalTable: "Lugar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
