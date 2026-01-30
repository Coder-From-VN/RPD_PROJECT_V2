using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class addtrainnertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirebaseUid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tnEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tnPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tnCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_FirebaseUid",
                table: "Trainers",
                column: "FirebaseUid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainers");
        }
    }
}
