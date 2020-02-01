

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDbContext
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitLoverCloudDb : Migration
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
                });

            migrationBuilder.CreateTable(
                name: "LoverAlbum",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    LoverGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverAlbum", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "LoverAnniversary",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    LoverGuid = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverAnniversary", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "LoverLog",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    Content = table.Column<string>(maxLength: 1024, nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    LoverGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverLog", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "LoverPhoto",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    PhotoTakenDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    AlbumGuid = table.Column<string>(nullable: true),
                    LoverGuid = table.Column<string>(nullable: true),
                    LoverLogGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverPhoto", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_LoverAlbum_AlbumGuid",
                        column: x => x.AlbumGuid,
                        principalTable: "LoverAlbum",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoverPhoto_LoverLog_LoverLogGuid",
                        column: x => x.LoverLogGuid,
                        principalTable: "LoverLog",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LoverAlbumGuid = table.Column<string>(nullable: true),
                    LoverPhotoGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Tag_LoverAlbum_LoverAlbumGuid",
                        column: x => x.LoverAlbumGuid,
                        principalTable: "LoverAlbum",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tag_LoverPhoto_LoverPhotoGuid",
                        column: x => x.LoverPhotoGuid,
                        principalTable: "LoverPhoto",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoverRequest",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    RequesterGuid = table.Column<string>(nullable: true),
                    ReceiverGuid = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    Succeed = table.Column<bool>(nullable: false),
                    LoverGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverRequest", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "LoverCloudUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
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
                    Birth = table.Column<DateTime>(nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    LoverRequestGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoverCloudUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoverCloudUser_LoverRequest_LoverRequestGuid",
                        column: x => x.LoverRequestGuid,
                        principalTable: "LoverRequest",
                        principalColumn: "Guid",
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
                name: "Lover",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    MaleGuid = table.Column<string>(nullable: true),
                    FemaleGuid = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    IsBoyFirstLove = table.Column<bool>(nullable: false),
                    IsGirlFirstLove = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lover", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Lover_LoverCloudUser_FemaleGuid",
                        column: x => x.FemaleGuid,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lover_LoverCloudUser_MaleGuid",
                        column: x => x.MaleGuid,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenstruationLog",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    LoverCloudUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenstruationLog", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_MenstruationLog_LoverCloudUser_LoverCloudUserId",
                        column: x => x.LoverCloudUserId,
                        principalTable: "LoverCloudUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenstruationDescription",
                columns: table => new
                {
                    Guid = table.Column<string>(type: "varchar(36)", nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    MenstruationLogGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenstruationDescription", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_MenstruationDescription_MenstruationLog_MenstruationLogGuid",
                        column: x => x.MenstruationLogGuid,
                        principalTable: "MenstruationLog",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Lover_FemaleGuid",
                table: "Lover",
                column: "FemaleGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lover_MaleGuid",
                table: "Lover",
                column: "MaleGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverAlbum_LoverGuid",
                table: "LoverAlbum",
                column: "LoverGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverAnniversary_LoverGuid",
                table: "LoverAnniversary",
                column: "LoverGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverCloudUser_LoverRequestGuid",
                table: "LoverCloudUser",
                column: "LoverRequestGuid");

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
                name: "IX_LoverLog_LoverGuid",
                table: "LoverLog",
                column: "LoverGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_AlbumGuid",
                table: "LoverPhoto",
                column: "AlbumGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_LoverGuid",
                table: "LoverPhoto",
                column: "LoverGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverPhoto_LoverLogGuid",
                table: "LoverPhoto",
                column: "LoverLogGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_LoverGuid",
                table: "LoverRequest",
                column: "LoverGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_ReceiverGuid",
                table: "LoverRequest",
                column: "ReceiverGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoverRequest_RequesterGuid",
                table: "LoverRequest",
                column: "RequesterGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenstruationDescription_MenstruationLogGuid",
                table: "MenstruationDescription",
                column: "MenstruationLogGuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_LoverCloudUser_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverAlbum_Lover_LoverGuid",
                table: "LoverAlbum",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverAnniversary_Lover_LoverGuid",
                table: "LoverAnniversary",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverLog_Lover_LoverGuid",
                table: "LoverLog",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverPhoto_Lover_LoverGuid",
                table: "LoverPhoto",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverRequest_LoverCloudUser_ReceiverGuid",
                table: "LoverRequest",
                column: "ReceiverGuid",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverRequest_LoverCloudUser_RequesterGuid",
                table: "LoverRequest",
                column: "RequesterGuid",
                principalTable: "LoverCloudUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoverRequest_Lover_LoverGuid",
                table: "LoverRequest",
                column: "LoverGuid",
                principalTable: "Lover",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverCloudUser_FemaleGuid",
                table: "Lover");

            migrationBuilder.DropForeignKey(
                name: "FK_Lover_LoverCloudUser_MaleGuid",
                table: "Lover");

            migrationBuilder.DropForeignKey(
                name: "FK_LoverRequest_LoverCloudUser_ReceiverGuid",
                table: "LoverRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LoverRequest_LoverCloudUser_RequesterGuid",
                table: "LoverRequest");

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
                name: "LoverRequest");

            migrationBuilder.DropTable(
                name: "Lover");
        }
    }
}
