using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddPropertyToLoverCloudUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPhysicalPath",
                table: "LoverPhoto");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalPath",
                table: "LoverPhoto",
                maxLength: 5120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageId",
                table: "Lover",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_CoverImageId",
                table: "Lover",
                column: "CoverImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverPhoto_CoverImageId",
                table: "Lover",
                column: "CoverImageId",
                principalTable: "LoverPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverPhoto_CoverImageId",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_CoverImageId",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "PhysicalPath",
                table: "LoverPhoto");

            migrationBuilder.DropColumn(
                name: "CoverImageId",
                table: "Lover");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPhysicalPath",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                maxLength: 5120,
                nullable: true);
        }
    }
}
