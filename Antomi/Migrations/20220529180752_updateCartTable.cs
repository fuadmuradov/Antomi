using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class updateCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Carts",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Carts",
                newName: "ProductColorId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                newName: "IX_Carts_ProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_ProductColors_ProductColorId",
                table: "Carts",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_ProductColors_ProductColorId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ProductColorId",
                table: "Carts",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Carts",
                newName: "Total");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_ProductColorId",
                table: "Carts",
                newName: "IX_Carts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Products_ProductId",
                table: "Carts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
