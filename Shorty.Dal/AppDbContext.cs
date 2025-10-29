namespace Shorty.Dal;
using Shorty.Dal.Models;
using Microsoft.EntityFrameworkCore;
public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Shorty> Shorties { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext()
    {
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shorty>()
            .HasIndex(s => s.ShortUrl)
            .IsUnique();

        modelBuilder.Entity<Shorty>()
            .HasOne(s => s.User)
            .WithMany(u => u._shorties)
            .HasForeignKey(s => s.UserId);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;" +
                                  "Username=neondb_owner;" +
                                  "Password=npg_5jmasY6vLIRC;" +
                                  "Database=yelenadb;" +
                                  "SSL Mode=Require;" +
                                  "Trust Server Certificate=true;" +
                                  "Channel Binding=Require;";

        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
    
    
}