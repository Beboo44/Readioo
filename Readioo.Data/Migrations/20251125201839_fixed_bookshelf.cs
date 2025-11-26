using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readioo.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixed_bookshelf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookShelves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BookShelves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
