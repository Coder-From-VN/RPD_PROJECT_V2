using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPD_API.Migrations
{
    public partial class updateexptype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "grTotalExp",
                table: "GrowthRate",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "grTotalExp",
                table: "GrowthRate",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
