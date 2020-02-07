using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class ModifyStringFieldLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_LoverAlbum_LoverAlbumGuid",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_LoverPhoto_LoverPhotoGuid",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_LoverAlbumGuid",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_LoverPhotoGuid",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "LoverAlbumGuid",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "LoverPhotoGuid",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "LoverAlbumId",
                table: "Tag",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoverPhotoId",
                table: "Tag",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "MenstruationDescription",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(36)");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "LoverPhoto",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LoverPhoto",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoverPhoto",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4096)",
                oldMaxLength: 4096,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LoverAlbum",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoverAlbum",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverAlbumId",
                table: "Tag",
                column: "LoverAlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverPhotoId",
                table: "Tag",
                column: "LoverPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_LoverAlbum_LoverAlbumId",
                table: "Tag",
                column: "LoverAlbumId",
                principalTable: "LoverAlbum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_LoverPhoto_LoverPhotoId",
                table: "Tag",
                column: "LoverPhotoId",
                principalTable: "LoverPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_LoverAlbum_LoverAlbumId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_LoverPhoto_LoverPhotoId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_LoverAlbumId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_LoverPhotoId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "LoverAlbumId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "LoverPhotoId",
                table: "Tag");

            migrationBuilder.AddColumn<string>(
                name: "LoverAlbumGuid",
                table: "Tag",
                type: "varchar(36) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoverPhotoGuid",
                table: "Tag",
                type: "varchar(36) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "MenstruationDescription",
                type: "varchar(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoverPhoto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LoverLog",
                type: "varchar(4096)",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4096,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LoverAlbum",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoverAlbum",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverAlbumGuid",
                table: "Tag",
                column: "LoverAlbumGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverPhotoGuid",
                table: "Tag",
                column: "LoverPhotoGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_LoverAlbum_LoverAlbumGuid",
                table: "Tag",
                column: "LoverAlbumGuid",
                principalTable: "LoverAlbum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_LoverPhoto_LoverPhotoGuid",
                table: "Tag",
                column: "LoverPhotoGuid",
                principalTable: "LoverPhoto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
