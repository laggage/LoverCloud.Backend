using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverCloudUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverCloudUser_FemaleGuid",
                table: "Lover");

            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverCloudUser_MaleGuid",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_FemaleGuid",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_MaleGuid",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "FemaleGuid",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "MaleGuid",
                table: "Lover");

            migrationBuilder.AddColumn<string>(
                name: "LoverGuid",
                table: "LoverCloudUser",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverCloudUser_LoverGuid",
                table: "LoverCloudUser",
                column: "LoverGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_LoverCloudUser_Lover_LoverGuid",
                table: "LoverCloudUser",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoverCloudUser_Lover_LoverGuid",
                table: "LoverCloudUser");

            migrationBuilder.DropIndex(
                name: "IX_LoverCloudUser_LoverGuid",
                table: "LoverCloudUser");

            migrationBuilder.DropColumn(
                name: "LoverGuid",
                table: "LoverCloudUser");

            migrationBuilder.AddColumn<string>(
                name: "FemaleGuid",
                table: "Lover",
                type: "varchar(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaleGuid",
                table: "Lover",
                type: "varchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_FemaleGuid",
                table: "Lover",
                column: "FemaleGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_MaleGuid",
                table: "Lover",
                column: "MaleGuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverCloudUser_FemaleGuid",
                table: "Lover",
                column: "FemaleGuid",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverCloudUser_MaleGuid",
                table: "Lover",
                column: "MaleGuid",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
