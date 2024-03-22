using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NIkitaBAD3.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a018285-956e-421c-b112-b567ca3f83be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d420a88f-1e88-422b-9991-0f18daa9a852");

            migrationBuilder.CreateTable(
                name: "PlayerGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameType = table.Column<int>(type: "int", nullable: false),
                    LongestCorrectAsnwerStreak = table.Column<int>(type: "int", nullable: false),
                    TempBestResult = table.Column<int>(type: "int", nullable: false),
                    TotalAnswers = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerGames_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12f07f19-2819-43d6-bc7c-c284a98728c1", null, "Administrator", "ADMINISTRATOR" },
                    { "846fa531-1330-434a-85c7-5a5abefd9f99", null, "Visitor", "VISITOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerGames_UserId",
                table: "PlayerGames",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerGames");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12f07f19-2819-43d6-bc7c-c284a98728c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "846fa531-1330-434a-85c7-5a5abefd9f99");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a018285-956e-421c-b112-b567ca3f83be", null, "Administrator", "ADMINISTRATOR" },
                    { "d420a88f-1e88-422b-9991-0f18daa9a852", null, "Visitor", "VISITOR" }
                });
        }
    }
}
