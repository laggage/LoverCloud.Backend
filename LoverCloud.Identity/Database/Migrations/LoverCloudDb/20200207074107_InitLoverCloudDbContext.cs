using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    public partial class InitLoverCloudDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lover",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    IsBoyFirstLove = table.Column<bool>(nullable: false),
                    IsGirlFirstLove = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lover", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoverAlbum",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    LoverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverAlbum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverAlbum_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoverAnniversary",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    LoverId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverAnniversary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverAnniversary_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoverCloudUser",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Birth = table.Column<DateTime>(type: "date", nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    ProfileImagePhysicalPath = table.Column<string>(nullable: true),
                    LoverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverCloudUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverCloudUser_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoverLog",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    Content = table.Column<string>(maxLength: 1024, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    LoverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverLog_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_LoverCloudUser_UserId",
                        column: x => x.UserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_LoverCloudUser_UserId",
                        column: x => x.UserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_LoverCloudUser_UserId",
                        column: x => x.UserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_LoverCloudUser_UserId",
                        column: x => x.UserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoverRequest",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    RequesterId = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Succeed = table.Column<bool>(nullable: true),
                    LoverId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverRequest_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoverRequest_LoverCloudUser_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoverRequest_LoverCloudUser_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenstruationLog",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LoverCloudUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenstruationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenstruationLog_LoverCloudUser_LoverCloudUserId",
                        column: x => x.LoverCloudUserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoverPhoto",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    PhotoTakenDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    PhotoPhysicalPath = table.Column<string>(nullable: true),
                    AlbumId = table.Column<string>(nullable: true),
                    LoverId = table.Column<string>(nullable: true),
                    LoverLogId = table.Column<string>(nullable: true),
                    UploaderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_LoverAlbum_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "LoverAlbum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_Lover_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Lover",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_LoverLog_LoverLogId",
                        column: x => x.LoverLogId,
                        principalTable: "LoverLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_LoverCloudUser_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenstruationDescription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    MenstruationLogId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenstruationDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenstruationDescription_MenstruationLog_MenstruationLogId",
                        column: x => x.MenstruationLogId,
                        principalTable: "MenstruationLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 30, nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LoverAlbumGuid = table.Column<string>(nullable: true),
                    LoverPhotoGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_LoverAlbum_LoverAlbumGuid",
                        column: x => x.LoverAlbumGuid,
                        principalTable: "LoverAlbum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tag_LoverPhoto_LoverPhotoGuid",
                        column: x => x.LoverPhotoGuid,
                        principalTable: "LoverPhoto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverAlbum_LoverId",
                table: "LoverAlbum",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverAnniversary_LoverId",
                table: "LoverAnniversary",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverCloudUser_LoverId",
                table: "LoverCloudUser",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "LoverCloudUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "LoverCloudUser",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverLog_LoverId",
                table: "LoverLog",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_AlbumId",
                table: "LoverPhoto",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_LoverId",
                table: "LoverPhoto",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_LoverLogId",
                table: "LoverPhoto",
                column: "LoverLogId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_UploaderId",
                table: "LoverPhoto",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_LoverId",
                table: "LoverRequest",
                column: "LoverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_ReceiverId",
                table: "LoverRequest",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_RequesterId",
                table: "LoverRequest",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_MenstruationDescription_MenstruationLogId",
                table: "MenstruationDescription",
                column: "MenstruationLogId");

            migrationBuilder.CreateIndex(
                name: "IX_MenstruationLog_LoverCloudUserId",
                table: "MenstruationLog",
                column: "LoverCloudUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverAlbumGuid",
                table: "Tag",
                column: "LoverAlbumGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_LoverPhotoGuid",
                table: "Tag",
                column: "LoverPhotoGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LoverAnniversary");

            migrationBuilder.DropTable(
                name: "LoverRequest");

            migrationBuilder.DropTable(
                name: "MenstruationDescription");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MenstruationLog");

            migrationBuilder.DropTable(
                name: "LoverPhoto");

            migrationBuilder.DropTable(
                name: "LoverAlbum");

            migrationBuilder.DropTable(
                name: "LoverLog");

            migrationBuilder.DropTable(
                name: "LoverCloudUser");

            migrationBuilder.DropTable(
                name: "Lover");
        }
    }
}
