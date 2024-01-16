using BlazorMovieLive.Server.Data;
using BlazorMovieLive.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovieLive.Authorization.Data
{
    public class BlazorMovieLiveDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlazorMovieLiveDbContext(DbContextOptions<BlazorMovieLiveDbContext> options) : base(options)
        {           

        }

        public DbSet<FavoriteMovie> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between User and Favorite
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Favorites)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);
        }
    }
}