using Microsoft.EntityFrameworkCore;
namespace Shorty.Dal.Data.Entities;

public class ApplicationDBContext : DbContext
{
	public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
		: base(options)
	{
	}
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Shorty> Shorties { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

		modelBuilder.Entity<Shorty>()
			.HasIndex(s => s.ShortUrl)
			.IsUnique();

		base.OnModelCreating(modelBuilder);
	}
}
