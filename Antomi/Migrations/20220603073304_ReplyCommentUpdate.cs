using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class ReplyCommentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "ReplyComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "ReplyComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReplyComments_AppUserId1",
                table: "ReplyComments",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyComments_AspNetUsers_AppUserId1",
                table: "ReplyComments",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplyComments_AspNetUsers_AppUserId1",
                table: "ReplyComments");

            migrationBuilder.DropIndex(
                name: "IX_ReplyComments_AppUserId1",
                table: "ReplyComments");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ReplyComments");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "ReplyComments");
        }
    }
}
