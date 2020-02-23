using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddLoverIdToLoverPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverPhoto_CoverImageId",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_CoverImageId",
                table: "Lover");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageId",
                table: "Lover",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(36) CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageId1",
                table: "Lover",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_CoverImageId1",
                table: "Lover",
                column: "CoverImageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverPhoto_CoverImageId1",
                table: "Lover",
                column: "CoverImageId1",
                principalTable: "LoverPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverPhoto_CoverImageId1",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_CoverImageId1",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "CoverImageId1",
                table: "Lover");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageId",
                table: "Lover",
                type: "varchar(36) CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
