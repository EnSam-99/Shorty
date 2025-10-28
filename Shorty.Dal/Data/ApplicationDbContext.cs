using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Shorty.Dal.Entities;
using System;

namespace Shorty.Dal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UrlMapping> UrlMappings { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → UrlMapping one-to-many
            modelBuilder.Entity<User>()
                .HasMany(u => u.UrlMappings)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure ShortURL is unique
            modelBuilder.Entity<UrlMapping>()
                .HasIndex(u => u.ShortURL)
                .IsUnique();
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;" +
                "Username=neondb_owner;" +
                "Password=npg_5jmasY6vLIRC;" +
                "Database=gagikdb;" +
                "SSL Mode=Require;" +
                "Trust Server Certificate=true;"
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

