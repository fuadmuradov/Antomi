using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class SubcategoryToMarkaTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Markas_SubCategories_SubCategoryId",
                table: "Markas");

            migrationBuilder.DropIndex(
                name: "IX_Markas_SubCategoryId",
                table: "Markas");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Markas");

            migrationBuilder.CreateTable(
                name: "SubcategoryToMarkas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    MarkaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubcategoryToMarkas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubcategoryToMarkas_Markas_MarkaId",
                        column: x => x.MarkaId,
                        principalTable: "Markas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubcategoryToMarkas_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubcategoryToMarkas_MarkaId",
                table: "SubcategoryToMarkas",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubcategoryToMarkas_SubCategoryId",
                table: "SubcategoryToMarkas",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubcategoryToMarkas");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Markas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Markas_SubCategoryId",
                table: "Markas",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Markas_SubCategories_SubCategoryId",
                table: "Markas",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
