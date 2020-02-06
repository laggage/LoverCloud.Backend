using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverCloudUserField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "LoverCloudUser");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePhysicalPath",
                table: "LoverCloudUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImagePhysicalPath",
                table: "LoverCloudUser");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "LoverCloudUser",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
