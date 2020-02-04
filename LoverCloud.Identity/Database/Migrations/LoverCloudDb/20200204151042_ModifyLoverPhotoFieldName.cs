using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverPhotoFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "LoverPhoto");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LoverPhoto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LoverPhoto");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
