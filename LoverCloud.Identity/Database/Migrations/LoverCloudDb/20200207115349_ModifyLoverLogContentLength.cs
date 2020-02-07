using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyLoverLogContentLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1024) CHARACTER SET utf8mb4",
                oldMaxLength: 1024,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                type: "varchar(1024) CHARACTER SET utf8mb4",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4096,
                oldNullable: true);
        }
    }
}
