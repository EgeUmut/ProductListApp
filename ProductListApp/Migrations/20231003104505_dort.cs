using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductListApp.Migrations
{
    /// <inheritdoc />
    public partial class dort : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ShoppingLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShoppingEndDate",
                table: "ShoppingLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShoppingStartDate",
                table: "ShoppingLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemCount",
                table: "Products",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "ShoppingEndDate",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "ShoppingStartDate",
                table: "ShoppingLists");

            migrationBuilder.DropColumn(
                name: "ItemCount",
                table: "Products");
        }
    }
}
