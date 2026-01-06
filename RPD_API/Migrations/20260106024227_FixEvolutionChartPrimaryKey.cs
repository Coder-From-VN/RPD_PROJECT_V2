using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class FixEvolutionChartPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EvolutionChart",
                table: "EvolutionChart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvolutionChart",
                table: "EvolutionChart",
                column: "evoID");

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionChart_pokeID_prePokeID",
                table: "EvolutionChart",
                columns: new[] { "pokeID", "prePokeID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EvolutionChart",
                table: "EvolutionChart");

            migrationBuilder.DropIndex(
                name: "IX_EvolutionChart_pokeID_prePokeID",
                table: "EvolutionChart");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvolutionChart",
                table: "EvolutionChart",
                columns: new[] { "pokeID", "prePokeID" });
        }
    }
}
