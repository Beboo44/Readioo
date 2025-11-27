using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readioo.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRatingToUserBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRating",
                table: "UserBooks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRating",
                table: "UserBooks");
        }
    }
}
