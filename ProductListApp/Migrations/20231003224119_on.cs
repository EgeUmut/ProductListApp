using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductListApp.Migrations
{
    /// <inheritdoc />
    public partial class on : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ShoppingLists_ShoppingListid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShoppingListid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShoppingListid",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductShoppingList",
                columns: table => new
                {
                    Productsid = table.Column<int>(type: "int", nullable: false),
                    shoppingListsid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShoppingList", x => new { x.Productsid, x.shoppingListsid });
                    table.ForeignKey(
                        name: "FK_ProductShoppingList_Products_Productsid",
                        column: x => x.Productsid,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShoppingList_ShoppingLists_shoppingListsid",
                        column: x => x.shoppingListsid,
                        principalTable: "ShoppingLists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductShoppingList_shoppingListsid",
                table: "ProductShoppingList",
                column: "shoppingListsid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductShoppingList");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingListid",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShoppingListid",
                table: "Products",
                column: "ShoppingListid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ShoppingLists_ShoppingListid",
                table: "Products",
                column: "ShoppingListid",
                principalTable: "ShoppingLists",
                principalColumn: "id");
        }
    }
}
