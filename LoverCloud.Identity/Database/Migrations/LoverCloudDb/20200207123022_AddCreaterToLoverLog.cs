using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddCreaterToLoverLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreaterId",
                table: "LoverLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverLog_CreaterId",
                table: "LoverLog",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoverLog_LoverCloudUser_CreaterId",
                table: "LoverLog",
                column: "CreaterId",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoverLog_LoverCloudUser_CreaterId",
                table: "LoverLog");

            migrationBuilder.DropIndex(
                name: "IX_LoverLog_CreaterId",
                table: "LoverLog");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "LoverLog");
        }
    }
}
