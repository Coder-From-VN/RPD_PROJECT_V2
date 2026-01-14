using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class EnablePokemonCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonAbilities_Abilities_abID",
                table: "PokemonAbilities");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonEggGroup_EggGroup_egID",
                table: "PokemonEggGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonGameVersion_GameVersion_gvID",
                table: "PokemonGameVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonMove_Move_moveID",
                table: "PokemonMove");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStats_StatType_stID",
                table: "PokemonStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonType_Types_typesID",
                table: "PokemonType");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonAbilities_Abilities_abID",
                table: "PokemonAbilities",
                column: "abID",
                principalTable: "Abilities",
                principalColumn: "abID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonEggGroup_EggGroup_egID",
                table: "PokemonEggGroup",
                column: "egID",
                principalTable: "EggGroup",
                principalColumn: "egID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonGameVersion_GameVersion_gvID",
                table: "PokemonGameVersion",
                column: "gvID",
                principalTable: "GameVersion",
                principalColumn: "gvID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonMove_Move_moveID",
                table: "PokemonMove",
                column: "moveID",
                principalTable: "Move",
                principalColumn: "moveID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStats_StatType_stID",
                table: "PokemonStats",
                column: "stID",
                principalTable: "StatType",
                principalColumn: "stID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonType_Types_typesID",
                table: "PokemonType",
                column: "typesID",
                principalTable: "Types",
                principalColumn: "typesID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonAbilities_Abilities_abID",
                table: "PokemonAbilities");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonEggGroup_EggGroup_egID",
                table: "PokemonEggGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonGameVersion_GameVersion_gvID",
                table: "PokemonGameVersion");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonMove_Move_moveID",
                table: "PokemonMove");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonStats_StatType_stID",
                table: "PokemonStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonType_Types_typesID",
                table: "PokemonType");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonAbilities_Abilities_abID",
                table: "PokemonAbilities",
                column: "abID",
                principalTable: "Abilities",
                principalColumn: "abID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonEggGroup_EggGroup_egID",
                table: "PokemonEggGroup",
                column: "egID",
                principalTable: "EggGroup",
                principalColumn: "egID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonGameVersion_GameVersion_gvID",
                table: "PokemonGameVersion",
                column: "gvID",
                principalTable: "GameVersion",
                principalColumn: "gvID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonMove_Move_moveID",
                table: "PokemonMove",
                column: "moveID",
                principalTable: "Move",
                principalColumn: "moveID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonStats_StatType_stID",
                table: "PokemonStats",
                column: "stID",
                principalTable: "StatType",
                principalColumn: "stID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonType_Types_typesID",
                table: "PokemonType",
                column: "typesID",
                principalTable: "Types",
                principalColumn: "typesID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
