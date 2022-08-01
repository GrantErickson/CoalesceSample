using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoalesceSample.Data.Migrations
{
    public partial class GameTagFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTag");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_GameId",
                table: "GameTags",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTags_TagId",
                table: "GameTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameTags_Games_GameId",
                table: "GameTags",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameTags_Tags_TagId",
                table: "GameTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameTags_Games_GameId",
                table: "GameTags");

            migrationBuilder.DropForeignKey(
                name: "FK_GameTags_Tags_TagId",
                table: "GameTags");

            migrationBuilder.DropIndex(
                name: "IX_GameTags_GameId",
                table: "GameTags");

            migrationBuilder.DropIndex(
                name: "IX_GameTags_TagId",
                table: "GameTags");

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
                        name: "FK_GameTag_Tags_GameTagsTagId",
                        column: x => x.GameTagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTag_GamesGameId",
                table: "GameTag",
                column: "GamesGameId");
        }
    }
}
