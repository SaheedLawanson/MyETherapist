using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace etherapist.Migrations
{
    public partial class UpdateCameronTest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CameronTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CameronTests");
        }
    }
}
