namespace Shorty.Dal;
using Shorty.Dal.Entities;
using Microsoft.EntityFrameworkCore;
public class AppDbContext:DbContext
{
   
    public DbSet<ShortyLink> Shorties { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<User> Users { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext()
    {
            
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortyLink>()
            .HasIndex(s => s.ShortUrl)
            .IsUnique();

        modelBuilder.Entity<ShortyLink>()
            .HasOne(s => s.User)
            .WithMany(u => u.ShortyLinks)
            .HasForeignKey(s => s.UserId);
        
        modelBuilder.Entity<ShortyLink>()
            .HasMany(s => s.Visits)
            .WithOne(v => v.ShortyLink)
            .HasForeignKey(v => v.ShortyId);
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