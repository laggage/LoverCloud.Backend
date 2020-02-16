using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddPropertyToLover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoveDayId",
                table: "Lover",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeddingDayId",
                table: "Lover",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_LoveDayId",
                table: "Lover",
                column: "LoveDayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_WeddingDayId",
                table: "Lover",
                column: "WeddingDayId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverAnniversary_LoveDayId",
                table: "Lover",
                column: "LoveDayId",
                principalTable: "LoverAnniversary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lover_LoverAnniversary_WeddingDayId",
                table: "Lover",
                column: "WeddingDayId",
                principalTable: "LoverAnniversary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverAnniversary_LoveDayId",
                table: "Lover");

            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverAnniversary_WeddingDayId",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_LoveDayId",
                table: "Lover");

            migrationBuilder.DropIndex(
                name: "IX_Lover_WeddingDayId",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "LoveDayId",
                table: "Lover");

            migrationBuilder.DropColumn(
                name: "WeddingDayId",
                table: "Lover");
        }
    }
}
