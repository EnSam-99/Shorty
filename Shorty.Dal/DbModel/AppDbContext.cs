using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;

namespace Shorty.Dal.Db;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ShortyModel> Shorties { get; set; } = null!;

    public DbSet<ShortyHistoryModel> Histories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;" +
                          "Username=neondb_owner;" +
                          "Password=npg_5jmasY6vLIRC;" +
                          "Database=arsendb;" +
                          "SSL Mode=Require;" +
                          "Trust Server Certificate=true;" +
                          "Channel Binding=Require;";

            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

    }

