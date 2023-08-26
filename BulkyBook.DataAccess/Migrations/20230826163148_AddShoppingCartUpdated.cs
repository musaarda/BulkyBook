using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCartUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_PoductId",
                table: "ShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "PoductId",
                table: "ShoppingCarts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_PoductId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShoppingCarts",
                newName: "PoductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_PoductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_PoductId",
                table: "ShoppingCarts",
                column: "PoductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
