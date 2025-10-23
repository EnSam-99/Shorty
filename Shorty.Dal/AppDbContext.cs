using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { } // Th

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Shorty> Shorties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shorty>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shorties)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
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
