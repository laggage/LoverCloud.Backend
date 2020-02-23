﻿// <auto-generated />
using System;
using LoverCloud.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoverCloud.Identity.Database.Migrations.LoverCloudDb
{
    [DbContext(typeof(LoverCloudDbContext))]
    partial class LoverCloudDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LoverCloud.Core.Models.Lover", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("CoverImageId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CoverImageId1")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<bool>("IsBoyFirstLove")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsGirlFirstLove")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LoveDayId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("WeddingDayId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("CoverImageId1");

                    b.HasIndex("LoveDayId")
                        .IsUnique();

                    b.HasIndex("WeddingDayId")
                        .IsUnique();

                    b.ToTable("Lover");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverAlbum", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreaterId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(512) CHARACTER SET utf8mb4")
                        .HasMaxLength(512);

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.HasIndex("LoverId");

                    b.ToTable("LoverAlbum");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverAnniversary", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("CreaterId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(512) CHARACTER SET utf8mb4")
                        .HasMaxLength(512);

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.HasIndex("LoverId");

                    b.ToTable("LoverAnniversary");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverCloudUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birth")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ProfileImagePhysicalPath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("LoverId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("LoverCloudUser");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverLog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasMaxLength(4096);

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreaterId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("CreaterId");

                    b.HasIndex("LoverId");

                    b.ToTable("LoverLog");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverPhoto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("AlbumId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(512) CHARACTER SET utf8mb4")
                        .HasMaxLength(512);

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("LoverLogId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.Property<DateTime>("PhotoTakenDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("varchar(512) CHARACTER SET utf8mb4")
                        .HasMaxLength(512);

                    b.Property<string>("PhysicalPath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasMaxLength(5120);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UploaderId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("LoverId");

                    b.HasIndex("LoverLogId");

                    b.HasIndex("UploaderId");

                    b.ToTable("LoverPhoto");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverRequest", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<string>("LoverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("ReceiverId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RequesterId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<bool?>("Succeed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("LoverId")
                        .IsUnique();

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("LoverRequest");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.MenstruationDescription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(1024) CHARACTER SET utf8mb4")
                        .HasMaxLength(1024);

                    b.Property<string>("MenstruationLogId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("MenstruationLogId");

                    b.ToTable("MenstruationDescription");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.MenstruationLog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoverCloudUserId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("LoverCloudUserId");

                    b.ToTable("MenstruationLog");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.Tag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LoverAlbumId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("LoverPhotoId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(30) CHARACTER SET utf8mb4")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("LoverAlbumId");

                    b.HasIndex("LoverPhotoId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(36) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.Lover", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverPhoto", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId1");

                    b.HasOne("LoverCloud.Core.Models.LoverAnniversary", "LoveDay")
                        .WithOne()
                        .HasForeignKey("LoverCloud.Core.Models.Lover", "LoveDayId");

                    b.HasOne("LoverCloud.Core.Models.LoverAnniversary", "WeddingDay")
                        .WithOne()
                        .HasForeignKey("LoverCloud.Core.Models.Lover", "WeddingDayId");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverAlbum", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Creater")
                        .WithMany("LoverAlbums")
                        .HasForeignKey("CreaterId");

                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithMany("LoverAlbums")
                        .HasForeignKey("LoverId");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverAnniversary", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Creater")
                        .WithMany()
                        .HasForeignKey("CreaterId");

                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithMany("LoverAnniversaries")
                        .HasForeignKey("LoverId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverCloudUser", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithMany("LoverCloudUsers")
                        .HasForeignKey("LoverId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverLog", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Creater")
                        .WithMany("LoverLogs")
                        .HasForeignKey("CreaterId");

                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithMany("LoverLogs")
                        .HasForeignKey("LoverId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverPhoto", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverAlbum", "Album")
                        .WithMany("Photos")
                        .HasForeignKey("AlbumId");

                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithMany("LoverPhotos")
                        .HasForeignKey("LoverId");

                    b.HasOne("LoverCloud.Core.Models.LoverLog", "LoverLog")
                        .WithMany("LoverPhotos")
                        .HasForeignKey("LoverLogId");

                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Uploader")
                        .WithMany("LoverPhotos")
                        .HasForeignKey("UploaderId");
                });

            modelBuilder.Entity("LoverCloud.Core.Models.LoverRequest", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.Lover", "Lover")
                        .WithOne("LoverRequest")
                        .HasForeignKey("LoverCloud.Core.Models.LoverRequest", "LoverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Receiver")
                        .WithMany("ReceivedLoverRequests")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "Requester")
                        .WithMany("LoverRequests")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.MenstruationDescription", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.MenstruationLog", "MenstruationLog")
                        .WithMany("MenstruationDescriptions")
                        .HasForeignKey("MenstruationLogId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.MenstruationLog", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", "LoverCloudUser")
                        .WithMany("MenstruationLogs")
                        .HasForeignKey("LoverCloudUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LoverCloud.Core.Models.Tag", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverAlbum", "LoverAlbum")
                        .WithMany("Tags")
                        .HasForeignKey("LoverAlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LoverCloud.Core.Models.LoverPhoto", "LoverPhoto")
                        .WithMany("Tags")
                        .HasForeignKey("LoverPhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LoverCloud.Core.Models.LoverCloudUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
