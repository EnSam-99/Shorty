using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Shorty.Dal.Entities;
using Microsoft.Extensions.Configuration;

namespace Shorty.Dal
{
    public class AppDbContext : DbContext
    {

        public AppDbContext()
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ShortLink> Shorties { get; set; }
        public DbSet<Visit> Visits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortLink>()
                .HasOne(s => s.User)
                .WithMany(u => u.Shorties)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.ShortLink)
                .WithMany(s => s.Visits)
                .HasForeignKey(v => v.ShortLinkId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ep-patient-morning-agkih675-pooler.c-2.eu-central-1.aws.neon.tech;Username=neondb_owner;Password=npg_5jmasY6vLIRC;Database=gagikdb;SSL Mode=Require;Trust Server Certificate=true;Channel Binding=Require;");
            base.OnConfiguring(optionsBuilder);
        }

      
    }
}
