using Microsoft.EntityFrameworkCore;

namespace Shorty.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<User> Users => Set<User>();
		public DbSet<ShortLink> Shorties => Set<ShortLink>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortLink>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shorties)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ShortLink>()
	            .Property(s => s.Clicks)
	            .HasDefaultValue(0);

			modelBuilder.Entity<ShortLink>()
				.Property(s => s.LastAccessed)
				.HasColumnType("timestamp with time zone");
			base.OnModelCreating(modelBuilder);
        }
    }
}
