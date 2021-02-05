using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APICartao.Migrations
{
    public partial class IntsToBetterStringsRegex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cvc",
                table: "Cartao");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Cartao",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DataValidade",
                table: "Cartao",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Cvv",
                table: "Cartao",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cvv",
                table: "Cartao");

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "Cartao",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataValidade",
                table: "Cartao",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Cvc",
                table: "Cartao",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
