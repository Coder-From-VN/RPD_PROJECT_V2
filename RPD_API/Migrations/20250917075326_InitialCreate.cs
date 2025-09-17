using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    abID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    abName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    abDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    abEffect = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.abID);
                });

            migrationBuilder.CreateTable(
                name: "EggGroup",
                columns: table => new
                {
                    egID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    egName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EggGroup", x => x.egID);
                });

            migrationBuilder.CreateTable(
                name: "GameVersion",
                columns: table => new
                {
                    gvID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    gvName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gvGen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersion", x => x.gvID);
                });

            migrationBuilder.CreateTable(
                name: "GrowthRate",
                columns: table => new
                {
                    growthRateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    grName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    grTotalExp = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthRate", x => x.growthRateID);
                });

            migrationBuilder.CreateTable(
                name: "StatType",
                columns: table => new
                {
                    stID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatType", x => x.stID);
                });

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

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pokeNationalNumber = table.Column<int>(type: "int", nullable: false),
                    pokeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pokeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pokeSpecies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pokeHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pokeWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pokeCatchRate = table.Column<double>(type: "float", nullable: false),
                    pokeBaseFriendship = table.Column<int>(type: "int", nullable: false),
                    pokeBaseExp = table.Column<int>(type: "int", nullable: false),
                    pokeMaleRate = table.Column<double>(type: "float", nullable: false),
                    pokeFemaleRate = table.Column<double>(type: "float", nullable: false),
                    pokeEggCycles = table.Column<int>(type: "int", nullable: false),
                    pokeState = table.Column<int>(type: "int", nullable: false),
                    growthRateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.pokeID);
                    table.ForeignKey(
                        name: "FK_Pokemons_GrowthRate_growthRateID",
                        column: x => x.growthRateID,
                        principalTable: "GrowthRate",
                        principalColumn: "growthRateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Move",
                columns: table => new
                {
                    moveID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    moveName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    moveDamageClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    movePower = table.Column<int>(type: "int", nullable: false),
                    moveAccuracy = table.Column<double>(type: "float", nullable: false),
                    movePP = table.Column<int>(type: "int", nullable: false),
                    movePriority = table.Column<int>(type: "int", nullable: false),
                    moveDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move", x => x.moveID);
                    table.ForeignKey(
                        name: "FK_Move_Type_typeID",
                        column: x => x.typeID,
                        principalTable: "Type",
                        principalColumn: "typeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EffortValues",
                columns: table => new
                {
                    evID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    evStatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eValues = table.Column<int>(type: "int", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffortValues", x => x.evID);
                    table.ForeignKey(
                        name: "FK_EffortValues_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvolutionChart",
                columns: table => new
                {
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    prePokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    evoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    evoCondition = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionChart", x => new { x.pokeID, x.prePokeID });
                    table.ForeignKey(
                        name: "FK_EvolutionChart_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID");
                    table.ForeignKey(
                        name: "FK_EvolutionChart_Pokemons_prePokeID",
                        column: x => x.prePokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID");
                });

            migrationBuilder.CreateTable(
                name: "ImageLink",
                columns: table => new
                {
                    imgID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    imgLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLink", x => x.imgID);
                    table.ForeignKey(
                        name: "FK_ImageLink_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonAbilities",
                columns: table => new
                {
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    abID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    paHiddenCheck = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonAbilities", x => new { x.pokeID, x.abID });
                    table.ForeignKey(
                        name: "FK_PokemonAbilities_Abilities_abID",
                        column: x => x.abID,
                        principalTable: "Abilities",
                        principalColumn: "abID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonAbilities_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonEggGroup",
                columns: table => new
                {
                    egID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonEggGroup", x => new { x.pokeID, x.egID });
                    table.ForeignKey(
                        name: "FK_PokemonEggGroup_EggGroup_egID",
                        column: x => x.egID,
                        principalTable: "EggGroup",
                        principalColumn: "egID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonEggGroup_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonGameVersion",
                columns: table => new
                {
                    gvID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pgvDexNumber = table.Column<int>(type: "int", nullable: false),
                    pgvEntries = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonGameVersion", x => new { x.pokeID, x.gvID });
                    table.ForeignKey(
                        name: "FK_PokemonGameVersion_GameVersion_gvID",
                        column: x => x.gvID,
                        principalTable: "GameVersion",
                        principalColumn: "gvID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonGameVersion_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonStats",
                columns: table => new
                {
                    stID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Basevalue = table.Column<int>(type: "int", nullable: false),
                    minValue = table.Column<int>(type: "int", nullable: false),
                    MaxValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonStats", x => new { x.pokeID, x.stID });
                    table.ForeignKey(
                        name: "FK_PokemonStats_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonStats_StatType_stID",
                        column: x => x.stID,
                        principalTable: "StatType",
                        principalColumn: "stID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonType",
                columns: table => new
                {
                    typeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonType", x => new { x.pokeID, x.typeID });
                    table.ForeignKey(
                        name: "FK_PokemonType_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonType_Type_typeID",
                        column: x => x.typeID,
                        principalTable: "Type",
                        principalColumn: "typeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonMove",
                columns: table => new
                {
                    pokeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    moveID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    pmLearnMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pmLearnLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonMove", x => new { x.pokeID, x.moveID });
                    table.ForeignKey(
                        name: "FK_PokemonMove_Move_moveID",
                        column: x => x.moveID,
                        principalTable: "Move",
                        principalColumn: "moveID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonMove_Pokemons_pokeID",
                        column: x => x.pokeID,
                        principalTable: "Pokemons",
                        principalColumn: "pokeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EffortValues_pokeID",
                table: "EffortValues",
                column: "pokeID");

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionChart_prePokeID",
                table: "EvolutionChart",
                column: "prePokeID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageLink_pokeID",
                table: "ImageLink",
                column: "pokeID");

            migrationBuilder.CreateIndex(
                name: "IX_Move_typeID",
                table: "Move",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonAbilities_abID",
                table: "PokemonAbilities",
                column: "abID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonEggGroup_egID",
                table: "PokemonEggGroup",
                column: "egID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonGameVersion_gvID",
                table: "PokemonGameVersion",
                column: "gvID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonMove_moveID",
                table: "PokemonMove",
                column: "moveID");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_growthRateID",
                table: "Pokemons",
                column: "growthRateID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonStats_stID",
                table: "PokemonStats",
                column: "stID");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonType_typeID",
                table: "PokemonType",
                column: "typeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EffortValues");

            migrationBuilder.DropTable(
                name: "EvolutionChart");

            migrationBuilder.DropTable(
                name: "ImageLink");

            migrationBuilder.DropTable(
                name: "PokemonAbilities");

            migrationBuilder.DropTable(
                name: "PokemonEggGroup");

            migrationBuilder.DropTable(
                name: "PokemonGameVersion");

            migrationBuilder.DropTable(
                name: "PokemonMove");

            migrationBuilder.DropTable(
                name: "PokemonStats");

            migrationBuilder.DropTable(
                name: "PokemonType");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "EggGroup");

            migrationBuilder.DropTable(
                name: "GameVersion");

            migrationBuilder.DropTable(
                name: "Move");

            migrationBuilder.DropTable(
                name: "StatType");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "GrowthRate");
        }
    }
}
