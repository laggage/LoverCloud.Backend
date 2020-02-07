using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddFieldToLoverAlbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreaterId",
                table: "LoverAlbum",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverAlbum_CreaterId",
                table: "LoverAlbum",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoverAlbum_LoverCloudUser_CreaterId",
                table: "LoverAlbum",
                column: "CreaterId",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoverAlbum_LoverCloudUser_CreaterId",
                table: "LoverAlbum");

            migrationBuilder.DropIndex(
                name: "IX_LoverAlbum_CreaterId",
                table: "LoverAlbum");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "LoverAlbum");
        }
    }
}
