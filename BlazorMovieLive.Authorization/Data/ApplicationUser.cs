using Microsoft.AspNetCore.Identity;
using BlazorMovieLive.Server.Entities;

namespace BlazorMovieLive.Server.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<FavoriteMovie>? Favorites { get; set; } // User's list of favorite movies
    }
}
