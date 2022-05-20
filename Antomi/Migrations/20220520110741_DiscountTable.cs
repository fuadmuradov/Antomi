using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class DiscountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Products_ProductId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Discounts",
                newName: "ProductColorId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_ProductId",
                table: "Discounts",
                newName: "IX_Discounts_ProductColorId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_ProductColors_ProductColorId",
                table: "Discounts",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_ProductColors_ProductColorId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "ProductColorId",
                table: "Discounts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_ProductColorId",
                table: "Discounts",
                newName: "IX_Discounts_ProductId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Products_ProductId",
                table: "Discounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
