using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class updateWishlistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_ProductColors_ProductColorId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "ProducrColorId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "ProductColorId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_ProductColors_ProductColorId",
                table: "Wishlists",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_ProductColors_ProductColorId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "ProductColorId",
                table: "Wishlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProducrColorId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_ProductColors_ProductColorId",
                table: "Wishlists",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
