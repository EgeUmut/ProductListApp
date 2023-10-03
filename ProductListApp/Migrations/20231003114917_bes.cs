using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductListApp.Migrations
{
    /// <inheritdoc />
    public partial class bes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserStatus",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShoppingEnd",
                table: "ShoppingLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShoppingStart",
                table: "ShoppingLists",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShoppingEnd",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "ShoppingStart",
                table: "ShoppingLists");
        }
    }
}
