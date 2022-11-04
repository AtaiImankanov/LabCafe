using Microsoft.EntityFrameworkCore.Migrations;

namespace homework_64_Atai.Migrations
{
    public partial class AddedKorzina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaffeId",
                table: "Korzinas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaffeId",
                table: "Korzinas");
        }
    }
}
