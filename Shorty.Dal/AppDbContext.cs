using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal
{
    public class AppDbContext: DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Shorty> Shorties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Email).IsUnique();
            });
            modelBuilder.Entity<Models.Shorty>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(2048);
                entity.Property(e => e.ShortUrl).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.ShortUrl).IsUnique();
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;" +
                          "Username=neondb_owner;" +
                          "Password=npg_5jmasY6vLIRC;" +
                          "Database=neondb;" +
                          "SSL Mode=Require;" +
                          "Trust Server Certificate=true;" +
                          "Channel Binding=Require;";

            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

    }
}
