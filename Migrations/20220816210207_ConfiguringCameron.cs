using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace etherapist.Migrations
{
    public partial class ConfiguringCameron : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameronQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionAValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionBValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionCValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionDValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameronQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CameronTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameronTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameronTests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CameronTests_UserId",
                table: "CameronTests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameronQuestions");

            migrationBuilder.DropTable(
                name: "CameronTests");
        }
    }
}
