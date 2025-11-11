using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Entities;

namespace Shorty.Dal;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ShortyEntity> Shorties { get; set; }
    public DbSet<ShortyHistoryEntity> Histories { get; set; }
    public DbSet<VisitEntity> Visits { get; set; }
	public DbSet<ShortyClickCount> ShortyClickCounts { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
