using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class AddCreaterToAnniversary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreaterId",
                table: "LoverAnniversary",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverAnniversary_CreaterId",
                table: "LoverAnniversary",
                column: "CreaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoverAnniversary_LoverCloudUser_CreaterId",
                table: "LoverAnniversary",
                column: "CreaterId",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoverAnniversary_LoverCloudUser_CreaterId",
                table: "LoverAnniversary");

            migrationBuilder.DropIndex(
                name: "IX_LoverAnniversary_CreaterId",
                table: "LoverAnniversary");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "LoverAnniversary");
        }
    }
}
