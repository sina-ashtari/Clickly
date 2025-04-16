using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clickly.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Favorite_Wrong_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_UserId",
                table: "Favorites");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
