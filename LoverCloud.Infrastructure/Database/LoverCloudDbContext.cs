namespace LoverCloud.Infrastructure.Database
{
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class LoverCloudDbContext : IdentityDbContext<LoverCloudUser>
    {
        public LoverCloudDbContext(DbContextOptions<LoverCloudDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Lover> Lovers { get; set; }
        public DbSet<LoverAnniversary> LoverAnniversaries { get; set; }
        public DbSet<LoverLog> LoverLogs { get; set; }
        public DbSet<LoverAlbum> LoverAlbums { get; set; }
        public DbSet<LoverPhoto> LoverPhotos { get; set; }
        public DbSet<LoverRequest> LoverRequests { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MenstruationLog> MenstruationLogs { get; set; }
        public DbSet<MenstruationDescription> MenstruationDescriptions { get; set; }

        /// <summary>
        /// 读取文件中的数据库连接字符串
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>数据库连接字符串</returns>
        public static string GetConnectionStringFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"file: {filePath}");

            return File.ReadAllText(filePath);
        }
    }
}
