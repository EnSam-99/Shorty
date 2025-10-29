namespace Shorty.Dal;
using Shorty.Dal.Entities;
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
            .WithMany(u => u.Shorties)
            .HasForeignKey(s => s.UserId);
    }
    
}