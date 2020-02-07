using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverLogContentLength2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                type: "varchar(4096)",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldMaxLength: 4096,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                type: "longtext CHARACTER SET utf8mb4",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4096)",
                oldMaxLength: 4096,
                oldNullable: true);
        }
    }
}
