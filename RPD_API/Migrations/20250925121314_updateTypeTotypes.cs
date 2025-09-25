using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class updateTypeTotypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Move_Type_typeID",
                table: "Move");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonType_Type_typeID",
                table: "PokemonType");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    typeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    typeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.typeID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Move_Types_typeID",
                table: "Move",
                column: "typeID",
                principalTable: "Types",
                principalColumn: "typeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonType_Types_typeID",
                table: "PokemonType",
                column: "typeID",
                principalTable: "Types",
                principalColumn: "typeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Move_Types_typeID",
                table: "Move");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonType_Types_typeID",
                table: "PokemonType");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    typeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    typeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.typeID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Move_Type_typeID",
                table: "Move",
                column: "typeID",
                principalTable: "Type",
                principalColumn: "typeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonType_Type_typeID",
                table: "PokemonType",
                column: "typeID",
                principalTable: "Type",
                principalColumn: "typeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
