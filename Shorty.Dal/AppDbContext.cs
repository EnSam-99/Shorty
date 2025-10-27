using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shorty.Dal.Models;

namespace Shorty.Dal;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ShortyModel> Shorties { get; set; } = null!;
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

     
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();

         
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("now() at time zone 'utc'");
        });

       
        modelBuilder.Entity<ShortyModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(2048);
            entity.Property(e => e.ShortUrl).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.ShortUrl).IsUnique();

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("now() at time zone 'utc'");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Shorties)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
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

