using BlazorMovieLive.Server.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovieLive.Authorization.Data
{
    public class BlazorMovieLiveDbContext : IdentityDbContext<ApplicationUser>
    {
        public BlazorMovieLiveDbContext(DbContextOptions<BlazorMovieLiveDbContext> options) : base(options)
        {

        }
    }
}