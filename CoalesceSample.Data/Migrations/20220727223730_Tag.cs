using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoalesceSample.Data.Migrations
{
    public partial class Tag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "GameTag",
                columns: table => new
                {
                    GameTagsTagId = table.Column<int>(type: "int", nullable: false),
                    GamesGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTag", x => new { x.GameTagsTagId, x.GamesGameId });
                    table.ForeignKey(
                        name: "FK_GameTag_Games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameTag_Tag_GameTagsTagId",
                        column: x => x.GameTagsTagId,
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTag_GamesGameId",
                table: "GameTag",
                column: "GamesGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTag");

            migrationBuilder.DropTable(
                name: "Tag");
        }
    }
}
