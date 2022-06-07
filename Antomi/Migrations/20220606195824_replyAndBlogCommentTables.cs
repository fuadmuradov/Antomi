using Microsoft.EntityFrameworkCore.Migrations;

namespace Antomi.Migrations
{
    public partial class replyAndBlogCommentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId1",
                table: "BlogComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId1",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "BlogComments");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ReplyComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "BlogComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyComments_AppUserId",
                table: "ReplyComments",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReplyComments_AspNetUsers_AppUserId",
                table: "ReplyComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReplyComments_AspNetUsers_AppUserId",
                table: "ReplyComments");

            migrationBuilder.DropIndex(
                name: "IX_ReplyComments_AppUserId",
                table: "ReplyComments");

            migrationBuilder.DropIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ReplyComments");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "BlogComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "BlogComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_AppUserId1",
                table: "BlogComments",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId1",
                table: "BlogComments",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
