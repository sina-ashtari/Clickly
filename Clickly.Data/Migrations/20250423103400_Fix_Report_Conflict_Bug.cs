using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clickly.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Report_Conflict_Bug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_UserId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
