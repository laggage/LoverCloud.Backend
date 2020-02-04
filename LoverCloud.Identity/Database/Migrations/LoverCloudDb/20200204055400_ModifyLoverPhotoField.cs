using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverPhotoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "LoverPhoto");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPhysicalPath",
                table: "LoverPhoto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "LoverPhoto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "LoverPhoto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_UploaderId",
                table: "LoverPhoto",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoverPhoto_LoverCloudUser_UploaderId",
                table: "LoverPhoto",
                column: "UploaderId",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoverPhoto_LoverCloudUser_UploaderId",
                table: "LoverPhoto");

            migrationBuilder.DropIndex(
                name: "IX_LoverPhoto_UploaderId",
                table: "LoverPhoto");

            migrationBuilder.DropColumn(
                name: "PhotoPhysicalPath",
                table: "LoverPhoto");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "LoverPhoto");

            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "LoverPhoto");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
