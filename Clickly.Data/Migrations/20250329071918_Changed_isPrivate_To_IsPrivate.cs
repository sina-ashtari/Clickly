using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clickly.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changed_isPrivate_To_IsPrivate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPrivate",
                table: "Posts",
                newName: "IsPrivate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPrivate",
                table: "Posts",
                newName: "isPrivate");
        }
    }
}
