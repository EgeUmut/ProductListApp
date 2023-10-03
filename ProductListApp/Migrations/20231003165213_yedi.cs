using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductListApp.Migrations
{
    /// <inheritdoc />
    public partial class yedi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Messages");
        }
    }
}
